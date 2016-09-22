using System;
using GalaSoft.MvvmLight;
using OxyPlot;
using OxyPlot.Axes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AkkaCPUReader;
using OxyPlot.Series;

namespace WpfCharting.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private PlotModel _plotModel;

        public PlotModel PlotModel
        {
            get { return _plotModel; }
            set { Set(() => PlotModel, ref _plotModel, value); }
        }

        private float _currentValue;

        public float CurrentValue
        {
            get { return _currentValue; }
            set { Set(() => CurrentValue, ref _currentValue, value); }
        }


        public MainWindowViewModel()
        {
            SetupChartModel();
            Action<float, DateTime> dataPointSetter = new Action<float, DateTime>((v, d) => SetDataPoint(v, d));

            ActorSystemReference.CreateActorSystem(dataPointSetter);
        }

        private void SetDataPoint(float value, DateTime date)
        {
            CurrentValue = value;
            UpdateLineSeries(value, date);
        }

        private void UpdateLineSeries(float value, DateTime date)
        {
            var series = _plotModel.Series[0] as LineSeries;

            var newDataPoint = new DataPoint(DateTimeAxis.ToDouble(date),
                LinearAxis.ToDouble(value));

            //show only last 20 datapoints
            if (series.Points.Count > 20)
            {
                series.Points.RemoveAt(0);
            }

            series.Points.Add(newDataPoint);
            _plotModel.InvalidatePlot(true);
        }

        private void SetupChartModel()
        {
            _plotModel = new PlotModel
            {
                LegendTitle = "Legend",
                LegendOrientation = LegendOrientation.Horizontal,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.TopRight,
                LegendBackground = OxyColor.FromAColor(200, OxyColors.White),
                LegendBorder = OxyColors.Black
            };

            var stockDateTimeAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Title = "Date",
                StringFormat = "HH:mm:ss"
            };

            _plotModel.Axes.Add(stockDateTimeAxis);

            var stockPriceAxis = new LinearAxis
            {
                Minimum = 0,
                Maximum = 100,
                IsZoomEnabled = false,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Title = "% CPU"
            };

            _plotModel.Axes.Add(stockPriceAxis);

            var newLineSeries = new LineSeries()
            {
                StrokeThickness = 2,
                MarkerSize = 3,
                MarkerStroke = OxyColors.Black,
                MarkerType = MarkerType.None,
                CanTrackerInterpolatePoints = false,
                Title = "CPU",
                Smooth = false
            };

            _plotModel.Series.Add(newLineSeries);
        }
    }
}