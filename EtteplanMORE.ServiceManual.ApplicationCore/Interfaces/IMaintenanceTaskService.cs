using EtteplanMORE.ServiceManual.ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Interfaces
{
    public interface IMaintenanceTaskService
    {
        Task<List<MaintenanceTasks>> Get();
        Task<List<MaintenanceTasks>> GetDeviceTask(int deviceId);
        Task<MaintenanceTasks> GetTask(int taskId);
        Task<MaintenanceTasks> DeleteTask(int taskId);
        Task<MaintenanceTasks> UpdateTask(MaintenanceTasks task);
        Task<MaintenanceTasks> AddMaintenanceTask(MaintenanceTasks task);
    }
}
