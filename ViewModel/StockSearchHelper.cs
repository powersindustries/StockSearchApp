using Newtonsoft.Json.Linq;
using Stocks.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Stocks.ViewModel.Helpers
{
    public class StockSearchHelper
    {
        // ----------------------------------------------------------------
        // Get a free API key at: https://www.alphavantage.co/support/#api-key
        // ----------------------------------------------------------------
        private const string API_KEY = "YOUR API KEY HERE";
        // ----------------------------------------------------------------
        // ----------------------------------------------------------------

        private const string m_sCompanyDataBaseURL = "https://www.alphavantage.co/query?function=OVERVIEW&symbol={0}&apikey={1}";
        private const string m_sStockBaseURL = "https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={0}&interval=5min&apikey={1}";

        private static CompanyData m_NotFoundData = new CompanyData("NOT_FOUND", "Symbol not found.", "", "", "", "");


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public static async Task<CompanyData> GetCompanyData(string symbol)
        {
            CompanyData output = new CompanyData();
            using (HttpClient httpClient = new HttpClient())
            {
                string CallURL = String.Format(m_sCompanyDataBaseURL, symbol, API_KEY);

                HttpResponseMessage response = await httpClient.GetAsync(CallURL);
                if (response.IsSuccessStatusCode)
                {
                    string responseJSON = await response.Content.ReadAsStringAsync();
                    JObject responseJObject = JObject.Parse(responseJSON);

                    if (responseJObject != null)
                    {
                        output.Symbol = "Symbol: " + responseJObject.Value<string>("Symbol");
                        output.Name = responseJObject.Value<string>("Name");
                        output.Description = responseJObject.Value<string>("Description");
                        output.Currency = responseJObject.Value<string>("Currency");
                        output.Country = "Country of Origin: " + responseJObject.Value<string>("Country");
                        output.Industry = "Industry: " + responseJObject.Value<string>("Industry");

                        if (output.Symbol == "Symbol: ")
                        {
                            output = m_NotFoundData;
                        }
                    }
                    else
                    {
                        output = m_NotFoundData;
                    }
                }
                else
                {
                    output = m_NotFoundData;
                }
            }

            return output;
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public static async Task<List<StockData>> GetStock(string symbol)
        {
            List<StockData> output = new List<StockData>();
            using (HttpClient httpClient = new HttpClient())
            {
                string CallURL = String.Format(m_sStockBaseURL, symbol, API_KEY);

                HttpResponseMessage response = await httpClient.GetAsync(CallURL);
                if (response.IsSuccessStatusCode)
                {
                    string responseJSON = await response.Content.ReadAsStringAsync();
                    JObject responseJObject = JObject.Parse(responseJSON);

                    if (responseJObject != null && responseJObject["Meta Data"] != null && responseJObject["Time Series (5min)"] != null)
                    {
                        foreach (JToken obj in responseJObject["Time Series (5min)"])
                        {
                            if (obj.First != null)
                            {
                                StockData newStockData = new StockData();
                                newStockData.Timestamp = DateTime.Parse(((JProperty)obj).Name);
                                newStockData.Open = obj.First.Value<double>("1. open");
                                newStockData.High = obj.First.Value<double>("2. high");
                                newStockData.Low = obj.First.Value<double>("3. low");
                                newStockData.Close = obj.First.Value<double>("4. close");
                                newStockData.Volume = obj.First.Value<long>("5. volume");

                                output.Add(newStockData);
                            }
                        }
                    }
                }
            }

            output.Reverse();
            return output;
        }
    }
}
