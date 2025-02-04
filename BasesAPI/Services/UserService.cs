using BasesAPI.Models;
using BasesAPI.Models.DTOs;
using BasesAPI.Services;
using Microsoft.AspNetCore.Identity; 
using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher<User> _passwordHasher;

    public UserService(AppDbContext context)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<bool> Register(User user)
    {

        if (await _context.Users.AnyAsync(u => u.Username == user.Username))
            return false;
        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            return false;

        user.PasswordHash = _passwordHasher.HashPassword(user, user.PasswordHash);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<User?> Login(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            return null;
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return user;
    }

    public async Task<bool> ModifyEmail(int userId,string email)
    {

        var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == userId);

        if (user == null) return false;

        _context.Users.Attach(user);       // Adjunta la entidad al contexto

        // Modificar solo una propiedad
        _context.Entry(user).Property(e => e.Email).CurrentValue = email;
        _context.Entry(user).Property(e => e.Email).IsModified = true;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<ChangePassResponse?> ModifyPass(int userId,string currentPass , string newPass)
    {
        var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == userId);
        
        if (user == null) return null;

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, currentPass);
        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return new ChangePassResponse { MessageResponse = "Wrong Password" };
        }

        _context.Users.Attach(user);
        newPass = _passwordHasher.HashPassword(user, newPass);

        // Modificar solo password
        _context.Entry(user).Property(e => e.PasswordHash).CurrentValue = newPass;
        _context.Entry(user).Property(e => e.PasswordHash).IsModified = true;

        await _context.SaveChangesAsync();
        return new ChangePassResponse { MessageResponse = "Change Saved" };

    }
}


