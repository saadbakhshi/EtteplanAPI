using EtteplanMORE.ServiceManual.ApplicationCore.Entities;
using EtteplanMORE.ServiceManual.ApplicationCore.Enum;
using EtteplanMORE.ServiceManual.ApplicationCore.Interfaces;
using EtteplanMORE.ServiceManual.Web.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EtteplanMORE.ServiceManual.Web.Controllers
{
    [Route("api/[controller]")]
    public class MaintenanceTasksController : Controller
    {
        private readonly IMaintenanceTaskService _maintenanceTaskService;
        private readonly IFactoryDeviceService _factoryDeviceService;
        public MaintenanceTasksController(IMaintenanceTaskService maintenanceTaskService, IFactoryDeviceService factoryDeviceService)
        {
            _maintenanceTaskService = maintenanceTaskService;
            _factoryDeviceService = factoryDeviceService;
        }

        [HttpGet]
        public async Task<ActionResult<MaintenanceTaskDTO>> Get()    
        {
            var maintenanceTasks = await _maintenanceTaskService.Get();

            if (maintenanceTasks.Count == 0)
            {
                return NotFound("There are no Maintenance Tasks");
            }
            else
            {
                return Ok(maintenanceTasks.Select(mt => TaskObject(mt)));
            } 
        }

        [HttpGet("{searchBy}/{id}")]
        public async Task<ActionResult<MaintenanceTaskDTO>> GetTasks(string searchBy, int id)
        {
            if (searchBy == "device")
            {
                var device = await _factoryDeviceService.GetDevice(id);

                if (device == null)
                {
                    return NotFound();
                }
                else
                {
                    var maintenanceTask = await _maintenanceTaskService.GetDeviceTask(device.Id);

                    if (maintenanceTask.Count == 0)
                    {
                        return NotFound($"No task associated with DeviceId = {device.Id}");
                    }
                    else
                    {
                        return Ok(maintenanceTask.Select(mt => TaskObject(mt)));
                    }
                }
            }
            else if (searchBy == "task")
            {
                var maintenanceTask = await _maintenanceTaskService.GetTask(id);

                if (maintenanceTask == null)
                {
                    return NotFound($"No task with Id = {id}");
                }

                return Ok(TaskObject(maintenanceTask));
            }
            else return BadRequest($"Cannot search by {searchBy}");
            
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var maintenancetask = await _maintenanceTaskService.DeleteTask(id);
            if (maintenancetask == null)
            {
                return NotFound("No Task found");
            }
            return Ok("Task has been deleted");

        }

        [HttpPost]
        public async Task<ActionResult<MaintenanceTasks>> Create([FromBody] MaintenanceTaskDTO maintenancetask)
        {
            if (maintenancetask.DeviceId == 0)
            {
                return BadRequest("Enter a valid Device Id");
            }
            var device = await _factoryDeviceService.GetDevice(maintenancetask.DeviceId);
            
            if(device == null)
            {
                return BadRequest($"No device with Id = {maintenancetask.DeviceId}");
            }

            if (maintenancetask == null)
            {
                return BadRequest();
            }
            if (maintenancetask.TaskDescription.IsNullOrEmpty())
            {
                return BadRequest("Task desctiption is a required parameter");
            }
            else if (maintenancetask.TaskSeverity == null)
            {
                return BadRequest("Task severity is required");
            }
            else if(maintenancetask.TaskStatus == null)
            {
                return BadRequest("Task Status is required");
            }

            if (maintenancetask.TaskSeverity != null)
            {
                Enum.TryParse(typeof(Severity), maintenancetask.TaskSeverity, true, out object severityResult);

                if (severityResult == null)
                {
                    return BadRequest("Value entered for Severity can only be Critical, Important or Unimportant");
                }
            }

            if(maintenancetask.TaskStatus != null)
            {
                Enum.TryParse(typeof(Status), maintenancetask.TaskStatus, true, out object statusResult);

                if (statusResult == null)
                {
                    return BadRequest("Value entered for Status can only be Open or Close");
                }
            }

            var newtask = new MaintenanceTasks()
            {
                DeviceId = maintenancetask.DeviceId,
                TaskDescription = maintenancetask.TaskDescription,
                TaskSeverity = (Severity)Enum.Parse(typeof(Severity), maintenancetask.TaskSeverity, true),
                TaskStatus = (Status)Enum.Parse(typeof(Status), maintenancetask.TaskStatus, true)
            };

            var createdTask = await _maintenanceTaskService.AddMaintenanceTask(newtask);

            return CreatedAtAction(nameof(Get), new { id = createdTask.Id }, TaskObject(createdTask));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<MaintenanceTaskDTO>> Update(int id, [FromBody] MaintenanceTaskDTO maintenancetask)
        {
            if (id != maintenancetask.TaskId)
            {
                return BadRequest("TaskId mismatch");
            }
            var getTask = await _maintenanceTaskService.GetTask(id);

            if (getTask == null)
            {
                return NotFound($"Task with Id = {id} not found");
            }

            if (maintenancetask.DeviceId != 0)
            {
                var getDevice = await _factoryDeviceService.GetDevice(maintenancetask.DeviceId);

                if (getDevice == null)
                {
                    return NotFound($"Device with Id = {maintenancetask.DeviceId} not found");
                }
            }

            if (maintenancetask.TaskSeverity != null)
            {
                Enum.TryParse(typeof(Severity), maintenancetask.TaskSeverity, true, out object severityResult);

                if (severityResult == null)
                {
                    return BadRequest("Value entered for Severity can only be Critical, Important or Unimportant");
                }
            }
            if(maintenancetask.TaskStatus != null)
            {
                Enum.TryParse(typeof(Status), maintenancetask.TaskStatus, true, out object statusResult);

                if (statusResult == null)
                {
                    return BadRequest("Value entered for Status can only be Open or Close");
                }
            }

            var task = new MaintenanceTasks()
            {
                Id = maintenancetask.TaskId,
                DeviceId = maintenancetask.DeviceId==0?getTask.DeviceId:maintenancetask.DeviceId,
                TaskDescription = maintenancetask.TaskDescription.IsNullOrEmpty()?getTask.TaskDescription:maintenancetask.TaskDescription,
                TaskSeverity = (Severity)Enum.Parse(typeof(Severity), maintenancetask.TaskSeverity==null?getTask.TaskSeverity.ToString():maintenancetask.TaskSeverity, true),
                TaskStatus = (Status)Enum.Parse(typeof(Status), maintenancetask.TaskStatus==null?getTask.TaskStatus.ToString():maintenancetask.TaskStatus, true)
            };

            var updatedTask = await _maintenanceTaskService.UpdateTask(task);
            return Ok(TaskObject(updatedTask));
        }

        private MaintenanceTaskDTO TaskObject(MaintenanceTasks maintenanceTask)
        {
            return new MaintenanceTaskDTO()
            {
                TaskId = maintenanceTask.Id,
                TaskDescription = maintenanceTask.TaskDescription,
                DeviceId = maintenanceTask.DeviceId,
                RegisteredTime = Convert.ToString(maintenanceTask.RegisteredTime),
                TaskSeverity = maintenanceTask.TaskSeverity.ToString(),
                TaskStatus = maintenanceTask.TaskStatus.ToString()
            };
        }
    }
}
