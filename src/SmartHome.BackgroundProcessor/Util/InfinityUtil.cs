using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BackgroundProcessor.Util
{
    internal static class InfinityUtil
    {

        /// <summary>
        /// If enumerable is used <paramref name="limitBeforePause"/> within a period of <paramref name="limitTimeWindow"/>,
        /// the loop pauses for <paramref name="pauseTime"/> 
        /// </summary>
        /// <param name="limitBeforePause"></param>
        /// <param name="limitTimeWindow"></param>
        /// <param name="pauseTime"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        internal static async IAsyncEnumerable<int> BeyondAsync(int limitBeforePause,
                                                                TimeSpan limitTimeWindow,
                                                                TimeSpan pauseTime,
                                                                [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            int count = 1;
            DateTime firstTimestamp = DateTime.Now;
            while (!cancellationToken.IsCancellationRequested)
            {
                count++;
                if (count > limitBeforePause && (DateTime.Now - firstTimestamp) < limitTimeWindow)
                {
                    count = 0;
                    await Task.Delay(pauseTime, cancellationToken);
                    firstTimestamp = DateTime.Now;
                }

                yield return count;
            }
        }

    }
}
