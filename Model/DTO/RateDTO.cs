namespace RattingSystem.Model.DTO
{
    public class RateDTO
    {
        public Guid SessionId { get; set; }
        public string UserId { get; set; }
        public int SessionRateDegree { get; set; }
        public int PresenterRateDegree { get; set; }
        public string Comment { get; set; }
    }
}
