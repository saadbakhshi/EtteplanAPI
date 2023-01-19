using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Entities
{
    public class FactoryDevices
    {
        public int Id { get; set; }
        public string DeviceName { get; set; }
        public int DeviceYear { get; set; }
        public string DeviceType { get; set; }

    }
}
