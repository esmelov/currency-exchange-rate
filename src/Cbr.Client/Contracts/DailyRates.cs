using System;
using System.Collections.Generic;

namespace Cbr.Client.Contracts
{
    public class DailyRates
    {
        public DateTime Date { get; set; }
        public DateTime PreviousDate { get; set; }
        public Uri PreviousUrl { get; set; }
        public DateTime Timestamp { get; set; }
        public Dictionary<string, CurrencyInfo> Valute { get; set; }
    }
}
