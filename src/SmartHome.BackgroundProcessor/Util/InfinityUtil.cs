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

        internal static async IAsyncEnumerable<int> BeyondAsync(int limit, 
                                                               TimeSpan errorRateLimit,
                                                               TimeSpan retrySpacing,
                                                               CancellationToken cancellationToken = default)
        {
            int count = 1;
            DateTime firstTimestamp = DateTime.Now;
            while (!cancellationToken.IsCancellationRequested)
            {
                count++;
                if (count > limit && (DateTime.Now - firstTimestamp) < errorRateLimit)
                {
                    count = 0;
                    await Task.Delay(retrySpacing, cancellationToken);
                    firstTimestamp = DateTime.Now;
                }

                yield return count;
            }
        }

    }
}
