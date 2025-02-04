using System.ComponentModel.DataAnnotations;

namespace BasesAPI.Models.DTOs
{
    public class UserDto
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; }

        public string Name { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
    }

    public class UserResponseDto : UserDto
    {
        public int Id { get; set; }

    }

    public class ChangePass
    {
        public string CurrentPasswordHash { get; set; } = string.Empty;
        public string NewPasswordHash { get; set; } = string.Empty;
    }

    public class ChangePassResponse
    {
        public string? MessageResponse { get; set; }
    }


}
