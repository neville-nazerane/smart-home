using Microsoft.EntityFrameworkCore;
using SmartHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.ServerServices
{
    public partial class AppDbContext
    {

        public async Task SeedScenesAsync()
        {

            var enumScenes = Enum.GetNames<SceneName>();
            var dbScenes = await Scenes.ToListAsync();

            var toAdd = enumScenes.Where(s => !dbScenes.Select(d => d.Name).Contains(s))
                                  .Select(s => new Scene
                                  {
                                      Name = s
                                  })
                                  .ToList();
            var toRemove = dbScenes.Where(scene => !enumScenes.Contains(scene.Name)).ToList();

            await Scenes.AddRangeAsync(toAdd);
            Scenes.RemoveRange(toRemove);

            await SaveChangesAsync();

        }

    }
}
