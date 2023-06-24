using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Models
{

    [Index(nameof(SettingName))]
    public class Setting
    {

        public int Id { get; set; }

        [MaxLength(100)]
        public string SettingName { get; set; }

        [MaxLength(50)]
        public string SettingValue { get; set; }

    }
}
