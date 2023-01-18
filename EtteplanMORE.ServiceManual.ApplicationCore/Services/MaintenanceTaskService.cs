using EtteplanMORE.ServiceManual.ApplicationCore.Entities;
using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Services
{
    public class MaintenanceTaskService:IMaintenanceTaskService
    {
        private readonly AppDbContext _appDbContext;
        public MaintenanceTaskService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<MaintenanceTasks>> Get()
        {
            return await Task.FromResult(_appDbContext.MaintenanceTask.
                OrderBy(c=>c.TaskSeverity).ThenBy(s=>s.RegisteredTime).ToList());
        }
        public async Task<List<MaintenanceTasks>> GetDeviceTask(int id)
        {
            return await Task.FromResult(_appDbContext.MaintenanceTask.
                Where(device=>device.DeviceId==id).
                OrderBy(d=>d.TaskSeverity).ThenBy(d=>d.RegisteredTime).ToList());
        }
        public async Task<MaintenanceTasks> GetTask(int taskId)
        {
            return await _appDbContext.MaintenanceTask.FirstOrDefaultAsync(task => task.Id == taskId);
        }
        public async Task<MaintenanceTasks> DeleteTask(int taskId)
        {
            var result = await _appDbContext.MaintenanceTask
            .FirstOrDefaultAsync(task => task.Id== taskId);
            if (result != null)
            {
                _appDbContext.MaintenanceTask.Remove(result);
                await _appDbContext.SaveChangesAsync();
                return result;
            }

            return null;
        }
        public async Task<MaintenanceTasks> UpdateTask(MaintenanceTasks task)
        {
            var result = await _appDbContext.MaintenanceTask
                .FirstOrDefaultAsync(e => e.Id == task.Id);

            if (result != null)
            {
                result.DeviceId = task.DeviceId;
                result.TaskDescription = task.TaskDescription;
                result.TaskSeverity = task.TaskSeverity;
                result.TaskStatus = task.TaskStatus;

                await _appDbContext.SaveChangesAsync();
                
                return result;
            }

            return null;
        }
        public async Task<MaintenanceTasks> AddMaintenanceTask(MaintenanceTasks task)
        {
            task.RegisteredTime = DateTime.Now;
            var result = await _appDbContext.MaintenanceTask.AddAsync(task);
            await _appDbContext.SaveChangesAsync();
            return await GetTask(result.Entity.Id);
        }
    }
}
