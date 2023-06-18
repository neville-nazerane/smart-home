using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Models
{
    public class DeviceLog
    {

        public int Id { get; set; }

        [MaxLength(150)]
        public string DeviceId { get; set; }

        [Required]
        public DateTime? OccurredOn { get; set; }

        [Required]
        public DeviceType? DeviceType { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

    }
}
