using Microsoft.EntityFrameworkCore;
using SmartHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.ServerServices
{
    public partial class AppDbContext : DbContext
    {

        public DbSet<DeviceLog> DeviceLogs { get; set; }

        public DbSet<Scene> Scenes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

    }
}
