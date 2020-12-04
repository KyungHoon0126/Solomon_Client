using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;

namespace Solomon.Core.Home.ViewModel
{
    public class HomeViewModel
    {
        #region Properties
        public SeriesCollection GenderRatioPieCollection { get; set; }

        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
        public SeriesCollection AgeRatioColumnCollection { get; set; }
        #endregion

        #region Constructor
        public HomeViewModel()
        {
            InitVariables();
        }
        #endregion

        private void InitVariables()
        {
            GenderRatioPieCollection = new SeriesCollection();
            AgeRatioColumnCollection = new SeriesCollection();
        }

        public void LoadGenderRatioDatas()
        {
            GenderRatioPieCollection.Add(new PieSeries()
            {
                Title = "Male",
                Values = new ChartValues<ObservableValue> { new ObservableValue(52) },
                DataLabels = true
            });
            GenderRatioPieCollection.Add(new PieSeries()
            {
                Title = "FeMale",
                Values = new ChartValues<ObservableValue> { new ObservableValue(48) },
                DataLabels = true
            });
        }

        public void LoadAgeRatioDatas()
        {
            AgeRatioColumnCollection.Add(new ColumnSeries()
            {
                Title = "2015",
                Values = new ChartValues<double> { 10, 50, 39, 50 }
            });
            Formatter = value => value.ToString("N");
        }
    }
}
