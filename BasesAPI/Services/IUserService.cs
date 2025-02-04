using BasesAPI.Models;
using BasesAPI.Models.DTOs;

namespace BasesAPI.Services
{
    public interface IUserService
    {
        Task<bool> Register(User user);
        Task<User?> Login(string username, string password);
        Task<bool> ModifyEmail(int userId, string email);

        Task<ChangePassResponse?> ModifyPass(int userId, string currentPass, string newPass);

    }

}
