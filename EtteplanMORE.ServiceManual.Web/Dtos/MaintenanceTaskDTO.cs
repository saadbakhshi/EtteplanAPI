namespace EtteplanMORE.ServiceManual.Web.Dtos
{
    public class MaintenanceTaskDTO
    {
        private DateTime registeredTime; 
        public int TaskId { get; set; }
        public int DeviceId { get; set; }
        public string? TaskDescription { get; set; }
        public string RegisteredTime {
            get { return registeredTime.ToString("dd:MM:yyyy HH:mm"); }
            set { registeredTime = Convert.ToDateTime(value); } }
        public string? TaskSeverity { get; set; }
        public string? TaskStatus { get; set; }
        
    }
}
