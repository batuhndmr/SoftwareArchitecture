namespace SoftwareArchitecture.Domain.Entities

{
    public class HealthStatus
    {
        public bool IsHealthy { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
