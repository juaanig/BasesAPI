using BasesAPI.Models.DTOs;
using BasesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BasesAPI.Services
{
    public class ContactService : IContactService
    {
        private readonly AppDbContext _context;

        public ContactService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ContactResponseDto> AddContactAsync(int userId, ContactDto contactDto)
        {
            var contact = new Contact
            {
                FirstName = contactDto.FirstName,
                LastName = contactDto.LastName,
                BirthDate = contactDto.BirthDate,
                PhoneNumber = contactDto.PhoneNumber,
                UserId = userId
            };

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return new ContactResponseDto
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                BirthDate = contact.BirthDate,
                PhoneNumber = contact.PhoneNumber
            };
        }

        public async Task<IEnumerable<ContactResponseDto>> GetContactsAsync(int userId)
        {
            return await _context.Contacts
                .Where(c => c.UserId == userId)
                .Select(c => new ContactResponseDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    BirthDate = c.BirthDate,
                    PhoneNumber = c.PhoneNumber
                }).ToListAsync();
        }

        public async Task<ContactResponseDto> GetContactByIdAsync(int userId, int contactId)
        {
            var contact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Id == contactId);

            if (contact == null) return null;

            return new ContactResponseDto
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                BirthDate = contact.BirthDate,
                PhoneNumber = contact.PhoneNumber
            };
        }

        public async Task<bool> DeleteContactAsync(int userId, int contactId)
        {
            var contact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.Id == contactId);

            if (contact == null) return false;

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
