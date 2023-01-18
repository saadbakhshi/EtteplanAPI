using EtteplanMORE.ServiceManual.ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Interfaces
{
    public interface IFactoryDeviceService
    {
        Task<IEnumerable<FactoryDevices>> GetAll();

        Task<FactoryDevices> GetDevice(int id);
    }
}