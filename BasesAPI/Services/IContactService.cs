using BasesAPI.Models.DTOs;

namespace BasesAPI.Services
{
    public interface IContactService
    {
        Task<ContactResponseDto> AddContactAsync(int userId, ContactDto contactDto);
        Task<IEnumerable<ContactResponseDto>> GetContactsAsync(int userId);
        Task<ContactResponseDto> GetContactByIdAsync(int userId, int contactId);
        Task<bool> DeleteContactAsync(int userId, int contactId);
    }
}
