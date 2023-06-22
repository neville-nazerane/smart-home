using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.ServerServices
{
    public class ScenesService
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly AppDbContext _dbContext;
        SmartContext context;
        SmartContext Context => context ??= _serviceProvider.GetService<SmartContext>();

        SmartDevices Devices => Context.Devices;

        public ScenesService(IServiceProvider serviceProvider, AppDbContext dbContext)
        {
            _serviceProvider = serviceProvider;
            _dbContext = dbContext;
        }



    }
}
