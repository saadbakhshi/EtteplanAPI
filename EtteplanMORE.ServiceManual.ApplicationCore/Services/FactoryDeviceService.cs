using EtteplanMORE.ServiceManual.ApplicationCore.Entities;
using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Services
{
    public class FactoryDeviceService : IFactoryDeviceService
    {
        private readonly AppDbContext _appDbContext;
        public FactoryDeviceService()
        {
        }

        public FactoryDeviceService (AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<FactoryDevices>> GetAll()
        {
            return await Task.FromResult(_appDbContext.FactoryDevice);
        }
        public async Task<FactoryDevices> GetDevice(int id)
        {
            return await Task.FromResult(_appDbContext.FactoryDevice.FirstOrDefault(device => device.Id == id));
        }
    }
}