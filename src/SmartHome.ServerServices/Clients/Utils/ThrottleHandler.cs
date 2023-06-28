using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.ServerServices.Clients.Utils
{
    internal class ThrottleHandler
    {
        private readonly TimeSpan _timeSpan;
        private readonly int _limit;

        private int counter;
        private DateTime? startedOn;
        private TaskCompletionSource locker;

        public ThrottleHandler(TimeSpan timeSpan, int limit)
        {
            _timeSpan = timeSpan;
            _limit = limit;
        }

        public async Task WaitIfRequiredAsync()
        {
            if (locker is not null) await locker.Task;
            var innerLock = new TaskCompletionSource();
            locker = innerLock;

            if (startedOn is null || (DateTime.UtcNow - startedOn.Value) > _timeSpan)
            {
                startedOn = DateTime.UtcNow;
                counter = 0;
            }
            else
            {
                counter++;
                if (counter == _limit)
                    await Task.Delay(_timeSpan);
            }

            locker = null;
            innerLock.SetResult();
        }

    }
}
