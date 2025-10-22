using ContactList.DTOs;
using ContactList.Entities;
using ContactList.Interfaces;
using MiniValidation;

public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ContactService(IContactRepository contactRepository, ICategoryRepository categoryRepository)
    {
        _contactRepository = contactRepository;
        _categoryRepository = categoryRepository;
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

        var category = await _categoryRepository.GetCategoryByNameAsync(contactRequest.Category);
        if (category == null)
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

        int? subcategoryId = null;
        var subCategoryKey = contactRequest.SubCategory?.Trim();

        if (subCategoryKey != null)
        {
            if (category.Name.Equals("Business", StringComparison.OrdinalIgnoreCase))
            {
                var predefined = await _categoryRepository.GetSubcategoryByNameAsync(category.Id, subCategoryKey);
                if (predefined == null)
                {
                    return OperationResult<Contact>.ValidationFailed(new Dictionary<string, string[]>
                {
                    { nameof(contactRequest.SubCategory), new[] { $"Invalid subcategory for Business: {subCategoryKey}" } }
                });
                }
                subcategoryId = predefined.Id;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(subCategoryKey))
                {
                    var existing = await _categoryRepository.GetSubcategoryByNameAsync(category.Id, subCategoryKey);
                    if (existing != null)
                    {
                        subcategoryId = existing.Id;
                    }

                    var createdSubcategory = await _categoryRepository.CreateSubcategoryAsync(category.Id, subCategoryKey, subCategoryKey);
                    subcategoryId = createdSubcategory.Id;
                }
            }
        }

        var contact = new Contact
        {
            FirstName = contactRequest.FirstName,
            LastName = contactRequest.LastName,
            Email = contactRequest.Email,
            DateOfBirth = contactRequest.DateOfBirth,
            CategoryId = category.Id,
            SubcategoryId = subcategoryId,
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

        var category = await _categoryRepository.GetCategoryByNameAsync(request.Category);
        if (category == null)
        {
            return OperationResult<Contact>.ValidationFailed(new Dictionary<string, string[]>
            {
                { nameof(request.Category), new[] { $"Invalid category: {request.Category}" } }
            });
        }
        var allowedCategories = new[] { "Private", "Business", "Other" };
        if (!allowedCategories.Any(a => a.Equals(category.Name, StringComparison.OrdinalIgnoreCase)))
        {
            return OperationResult<Contact>.ValidationFailed(new Dictionary<string, string[]>
            {
                { nameof(request.Category), new[] { $"Invalid category: {request.Category}" } }
            });
        }

        int? subcategoryId = null;
        var subCategoryKey = request.SubCategory?.Trim();
        if (subCategoryKey != null)
        {
            if (category.Name.Equals("Business", StringComparison.OrdinalIgnoreCase))
            {
                var predefined = await _categoryRepository.GetSubcategoryByNameAsync(category.Id, subCategoryKey);
                if (predefined == null)
                {
                    return OperationResult<Contact>.ValidationFailed(new Dictionary<string, string[]>
                {
                    { nameof(request.SubCategory), new[] { $"Invalid subcategory for Business: {subCategoryKey}" } }
                });
                }
                subcategoryId = predefined.Id;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(subCategoryKey))
                {
                    var existing = await _categoryRepository.GetSubcategoryByNameAsync(category.Id, subCategoryKey);
                    if (existing != null)
                    {
                        subcategoryId = existing.Id;
                    }

                    var createdSubcategory = await _categoryRepository.CreateSubcategoryAsync(category.Id, subCategoryKey, subCategoryKey);
                    subcategoryId = createdSubcategory.Id;
                }
            }
        }

        dbContact.DateOfBirth = request.DateOfBirth;
        dbContact.FirstName = request.FirstName;
        dbContact.LastName = request.LastName;
        dbContact.Email = request.Email;
        dbContact.TelephoneNumber = request.TelephoneNumber;
        dbContact.CategoryId = category.Id;
        dbContact.SubcategoryId = subcategoryId;

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
