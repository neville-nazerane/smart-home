﻿using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.BackgroundProcessor.Util
{
    public class BackgroundQueue<T>
    {
        private readonly Queue<T> _queue;

        public BackgroundQueue()
        {
            _queue = new();
        }

        public void Enqueue(T item) => _queue.Enqueue(item);

        public IAsyncEnumerable<T> KeepDequeuingAsync(double spacingMiliseconds, CancellationToken cancellationToken = default)
            => KeepDequeuingAsync(TimeSpan.FromMilliseconds(spacingMiliseconds), cancellationToken);

        public async IAsyncEnumerable<T> KeepDequeuingAsync(TimeSpan spacing, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_queue.TryDequeue(out var item))
                    yield return item;
                else
                    await Task.Delay(spacing, cancellationToken);
            }
        }

    }
}
