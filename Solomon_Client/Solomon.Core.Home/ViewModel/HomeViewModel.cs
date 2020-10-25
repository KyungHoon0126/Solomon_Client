using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Solomon.Core.Home.ViewModel
{
    public class HomeViewModel
    {
        #region Properties
        public SeriesCollection SeriesCollection { get; set; }
        #endregion

        public HomeViewModel()
        {
            SeriesCollection = new SeriesCollection();

        }

        public void LoadChartDatas()
        {
            SeriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Male",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(52) },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "FeMale",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(48) },
                    DataLabels = true
                }
            };

            //adding values or series will update and animate the chart automatically
            //SeriesCollection.Add(new PieSeries());
            //SeriesCollection[0].Values.Add(5);
        }
    }
}
