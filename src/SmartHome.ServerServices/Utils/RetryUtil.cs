using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
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

        public async Task ExecuteAsync(Func<Task> func)
        {
            Exception exception = null;
            for (int i = 0; i < _count; i++)
            {
                try
                {
                    await func();
                    exception = null;
                    if (verifyFunc is null || await verifyFunc())
                        continue;
                }
                catch (Exception ex)
                {
                    exception = ex;
                    await Task.Delay(_spacing);
                }
            }
            if (exception is not null) throw exception;
        }

    }
}
