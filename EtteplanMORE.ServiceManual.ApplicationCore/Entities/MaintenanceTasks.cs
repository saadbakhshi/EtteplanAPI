using System;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Entities
{
    public class MaintenanceTasks
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public string TaskDescription { get; set; }
        public DateTime RegisteredTime { get; set; }
        public Enum.Severity TaskSeverity { get; set; }
        public Enum.Status TaskStatus { get; set; }
    }
}
