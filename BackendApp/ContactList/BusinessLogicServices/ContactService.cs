using ContactList.DTOs;
using ContactList.Entities;
using ContactList.Helpers;
using ContactList.Interfaces;
using MiniValidation;

public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;

    public ContactService(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<IEnumerable<Contact>> GetContactsAsync()
    {
        return await _contactRepository.GetAllContactsAsync();
    }

    public async Task<OperationResult<Contact>> GetContactByIdAsync(int id)
    {
        var contact = await _contactRepository.GetContactByIdAsync(id);
        return contact == null
            ? OperationResult<Contact>.NotFoundResult()
            : OperationResult<Contact>.Successful(contact);
    }

    public async Task<OperationResult<Contact>> CreateContactAsync(CreateContactRequest contactRequest)
    {
        if (!MiniValidator.TryValidate(contactRequest, out var errors))
        {
            return OperationResult<Contact>.ValidationFailed(errors);
        }

        if (!Enum.TryParse<ContactType>(contactRequest.Category, true, out var parsedCategory))
        {
            return OperationResult<Contact>.ValidationFailed(new Dictionary<string, string[]>
            {
                { nameof(contactRequest.Category), new[] { $"Invalid category: {contactRequest.Category}" } }
            });
        }

        if (await _contactRepository.GetContactByEmailAsync(contactRequest.Email) != null)
        {
            return OperationResult<Contact>.ValidationFailed(new Dictionary<string, string[]>
            {
                { nameof(contactRequest.Email), new[] { $"Contact with given email already exists" } }
            });
        }

        var contact = new Contact
        {
            FirstName = contactRequest.FirstName,
            LastName = contactRequest.LastName,
            Email = contactRequest.Email,
            DateOfBirth = contactRequest.DateOfBirth,
            Category = parsedCategory,
            SubCategory = contactRequest.SubCategory,
            TelephoneNumber = contactRequest.TelephoneNumber
        };

        await _contactRepository.InsertContactAsync(contact);
        return OperationResult<Contact>.Successful(contact);
    }

    public async Task<OperationResult<Contact>> UpdateContactAsync(EditContactRequest request)
    {
        if (!MiniValidator.TryValidate(request, out var errors))
        {
            return OperationResult<Contact>.ValidationFailed(errors);
        }

        var dbContact = await _contactRepository.GetContactByIdAsync(request.Id);
        if (dbContact == null)
        {
            return OperationResult<Contact>.NotFoundResult();
        }

        if (!Enum.TryParse<ContactType>(request.Category, true, out var parsedCategory))
        {
            return OperationResult<Contact>.ValidationFailed(new Dictionary<string, string[]>
            {
                { nameof(request.Category), new[] { $"Invalid category: {request.Category}" } }
            });
        }

        dbContact.DateOfBirth = request.DateOfBirth;
        dbContact.FirstName = request.FirstName;
        dbContact.LastName = request.LastName;
        dbContact.Email = request.Email;
        dbContact.TelephoneNumber = request.TelephoneNumber;
        dbContact.Category = parsedCategory;
        dbContact.SubCategory = request.SubCategory;

        await _contactRepository.SaveContactChangesAsync();
        return OperationResult<Contact>.Successful(dbContact);
    }

    public async Task<OperationResult<bool>> DeleteContactAsync(int id)
    {
        var contact = await _contactRepository.GetContactByIdAsync(id);
        if (contact == null)
        {
            return OperationResult<bool>.NotFoundResult();
        }

        await _contactRepository.DeleteContactAsync(contact);
        return OperationResult<bool>.Successful(true);
    }
}
