using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.ServerServices.Utils
{
    public class RetryUtil
    {
        private readonly int _count;
        private readonly TimeSpan _spacing;

        private Func<Task<bool>> verifyFunc; 

        public RetryUtil(int count, TimeSpan spacing)
        {
            _count = count;
            _spacing = spacing;
        }

        public static RetryUtil Setup(int count, TimeSpan spacing) => new(count, spacing);

        public RetryUtil SetVerification(Func<Task<bool>> verifyFunc)
        {
            this.verifyFunc = verifyFunc;
            return this;
        }

        public Task ExecuteAsync(Func<Task> func)
            => ExecuteAsync(async () =>
            {
                await func();
                return 0;
            });

        public async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> func)
        {
            Exception exception = new("Something went wrong");
            for (int i = 0; i < _count; i++)
            {
                try
                {
                    var res = await func();
                    exception = null;
                    if (verifyFunc is null || await verifyFunc())
                        return res;
                    throw new Exception("Failed to validate");
                }
                catch (Exception ex)
                {
                    if (ex is TaskCanceledException || ex is OperationCanceledException)
                        throw;
                    exception = ex;
                }
                await Task.Delay(_spacing);
            }
            throw exception;
        }

    }
}
