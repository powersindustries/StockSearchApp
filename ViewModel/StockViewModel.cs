using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Stocks.ViewModel.Commands;
using Stocks.ViewModel.Helpers;
using Stocks.Model;
using System.Windows;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;

namespace Stocks.ViewModel
{
    public enum GraphFormats
    {
        eOpen,
        eHigh,
        eLow,
        eClose,
        eVolume
    }

    public class StockViewModel : INotifyPropertyChanged
    {


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public StockViewModel()
        {
            StockData = new List<StockData>();
            CompanyData = new CompanyData();

            CompanySearchCommand = new CompanySearchCommand(this);
            FilterGraphCommand = new FilterGraphCommand(this);

            Visibility = Visibility.Collapsed;

            Series = new SeriesCollection()
            {
                new LiveCharts.Wpf.ColumnSeries
                {
                    Title="",
                    Values = new ChartValues<double>()
                }
            };

            XAxes = new AxesCollection
            {
                new LiveCharts.Wpf.Axis
                {
                    Title = "Purchase Time",
                    Foreground = Brushes.Gray,
                    Labels = new List<string>()
                },
            };

            YAxes = new AxesCollection
            {
                new LiveCharts.Wpf.Axis
                {
                    Title = "Y Axis",
                    Foreground = Brushes.Gray
                },
            };
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public AxesCollection XAxes { get; set; }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public AxesCollection YAxes { get; set; }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public SeriesCollection Series { get; set; }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public List<StockData> StockData { get; set; }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public CompanySearchCommand CompanySearchCommand { get; set; }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public FilterGraphCommand FilterGraphCommand { get; set; }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        private CompanyData m_CompanyData = new CompanyData();
        public CompanyData CompanyData
        {
            get
            {
                return m_CompanyData;
            }
            set
            {
                m_CompanyData = value;
                OnPropertyChanged("CompanyData");
            }
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        private string m_Query = "";
        public string Query
        {
            get
            {
                return m_Query;
            }
            set
            {
                m_Query = value.ToUpper();
                OnPropertyChanged("Query");
            }
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        private Visibility m_Visibility;
        public Visibility Visibility
        {
            get
            {
                return m_Visibility;
            }
            set
            {
                m_Visibility = value;
                OnPropertyChanged("Visibility");
            }
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public async void SearchCompany()
        {
            CompanyData = await StockSearchHelper.GetCompanyData(Query);

            if (CompanyData.Symbol != "NOT_FOUND")
            {
                MakeQuery();
            }
            else
            {
                Visibility = Visibility.Collapsed;

                if (Series[0].Values.Count > 0)
                {
                    Series[0].Values.Clear();
                }
            }
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public async void MakeQuery()
        {
            StockData.Clear();
            StockData = await StockSearchHelper.GetStock(Query);

            Series[0].Values.Clear();
            XAxes[0].Labels.Clear();

            for (int x = 0; x < StockData.Count(); ++x)
            {
                Series[0].Values.Add(StockData[x].Open);

                // Convert time to EST (New York Stock Exchange Timezone).
                TimeZoneInfo easternTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                DateTime convertedDateTime = TimeZoneInfo.ConvertTime(StockData[x].Timestamp, easternTimeZone);
                XAxes[0].Labels.Add(convertedDateTime.ToString("hh:mm tt"));
            }

            YAxes[0].Title = "Price (" + CompanyData.Currency + ")";

            Visibility = Visibility.Visible;
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public void FilterGraph(GraphFormats graphFormats)
        {
            if (Series[0].Values.Count > 0)
            {
                Series[0].Values.Clear();

                for (int x = 0; x < StockData.Count; ++x)
                {
                    switch (graphFormats)
                    {
                        case GraphFormats.eOpen:
                        {
                            Series[0].Values.Add(StockData[x].Open);
                            break;
                        }
                        case GraphFormats.eHigh:
                        {
                            Series[0].Values.Add(StockData[x].High);
                            break;
                        }
                        case GraphFormats.eLow:
                        {
                            Series[0].Values.Add(StockData[x].Low);
                            break;
                        }
                        case GraphFormats.eClose:
                        {
                            Series[0].Values.Add(StockData[x].Close);
                            break;
                        }
                        case GraphFormats.eVolume:
                        {
                            Series[0].Values.Add((double)StockData[x].Volume);
                            break;
                        }
                    }
                }
            }
        }
    }
}
