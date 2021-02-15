using Cbr.Client.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cbr.Client.Abstractions
{
    public interface ICbrClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<DailyRates> GetCurrentDayRates(CancellationToken token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="forDay"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<DailyRates> GetArchiveRates(DateTime forDay, CancellationToken token);
    }
}
