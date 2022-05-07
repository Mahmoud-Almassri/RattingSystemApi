namespace RattingSystem.Model.DTO
{
    public class SessionDTO
    {
        public Guid Id { get; set; }
        public string SessionName { get; set; }
        public string PresenterName { get; set; }
        public double AverageSessionRates { get; set; }
        public double AveragePresenterRates { get; set; }
    }
}
