using ContactList.DTOs;
using ContactList.Entities;

/// <summary>
/// Contact service for handling operations from API
/// </summary>
public interface IContactService
{
    Task<IEnumerable<Contact>> GetContactsAsync();
    Task<OperationResult<Contact>> GetContactByIdAsync(int id);
    Task<OperationResult<Contact>> CreateContactAsync(CreateContactRequest contactRequest);
    Task<OperationResult<Contact>> UpdateContactAsync(EditContactRequest request);
    Task<OperationResult<bool>> DeleteContactAsync(int id);
}
