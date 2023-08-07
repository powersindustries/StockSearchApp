using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Model
{
    public class CompanyData
    {


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public CompanyData()
        {
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public CompanyData(string symbol, string name, string description, string currency, string country, string industry)
        {
            Symbol = symbol;
            Name = name;
            Description = description;
            Currency = currency;
            Country = country;
            Industry = industry;
        }


        // ----------------------------------------------------------------
        // ----------------------------------------------------------------
        public string? Symbol { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Currency { get; set; }
        public string? Country { get; set; }
        public string? Industry { get; set; }
    }
}
