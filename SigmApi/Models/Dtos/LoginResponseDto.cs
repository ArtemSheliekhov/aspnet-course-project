namespace SigmApi.Models.Dtos
{
    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public string SessionId { get; set; }
        public string Username { get; set; }
    }
}
