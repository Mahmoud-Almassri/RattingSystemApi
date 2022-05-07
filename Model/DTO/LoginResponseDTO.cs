namespace RattingSystem.Model.DTO
{
    public class LoginResponseDTO
    {
        public string AccessToken { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public IList<string> Roles { get; set; }
    }
}
