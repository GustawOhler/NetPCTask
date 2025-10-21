using ContactList.DTOs;
using ContactList.Entities;

public interface IContactService
{
    Task<IEnumerable<Contact>> GetContactsAsync();
    Task<ContactOperationResult<Contact>> GetContactByIdAsync(int id);
    Task<ContactOperationResult<Contact>> CreateContactAsync(CreateContactRequest contactRequest);
    Task<ContactOperationResult<Contact>> UpdateContactAsync(EditContactRequest request);
    Task<ContactOperationResult<bool>> DeleteContactAsync(int id);
}
