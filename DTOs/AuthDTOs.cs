using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }

    public class AuthTokenDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public long ExpiresAt { get; set; }
        public string TokenType { get; set; } = "Bearer";
    }
}
