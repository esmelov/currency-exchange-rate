using Cbr.Client.Abstractions;
using Cbr.Client.Contracts;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Cbr.Client
{
    public class CbrClient : ICbrClient
    {
        private readonly HttpClient _client;

        public CbrClient(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<DailyRates> GetCurrentDayRates(CancellationToken token)
        {
            return await _client.GetFromJsonAsync<DailyRates>("daily_json.js", token);
        }

        public async Task<DailyRates> GetArchiveRates(DateTime forDay, CancellationToken token)
        {
            const string pathTemplate = "archive/{0}/{1:00}/{2:00}/daily_json.js";

            if (forDay.Date == DateTime.UtcNow.Date)
                return await GetCurrentDayRates(token);

            var uri = string.Format(pathTemplate, forDay.Date.Year, forDay.Month, forDay.Day);

            return await _client.GetFromJsonAsync<DailyRates>(
                uri, token);
        }
    }
}
