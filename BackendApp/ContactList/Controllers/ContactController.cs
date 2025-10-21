using ContactList.DTOs;
using ContactList.Entities;
using ContactList.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniValidation;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private IContactRepository _contactRepository;

    public ContactController(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }


    [HttpGet]
    public async Task<IActionResult> GetContacts()
    {
        return Ok(await _contactRepository.GetAllContactsAsync());
    }

    [HttpGet]
    public async Task<IActionResult> GetContactById(int id)
    {
        var contact = await _contactRepository.GetContactByIdAsync(id);
        if (contact == null)
        {
            return NotFound();
        }

        return Ok(contact);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateContact(CreateContactRequest contactRequest)
    {
        if (!MiniValidator.TryValidate(contactRequest, out var errors))
        {
            // return ValidationProblem(errors);
            return ValidationProblem();
        }

        if (!Enum.TryParse<ContactType>(contactRequest.Category, true, out var parsedCategory))
        {
            // return ValidationProblem(new Dictionary<string, string[]>() { { "Category", new string[] { $"Invalid category: {contactRequest.Category}" } } });
            return ValidationProblem();
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
        return Created($"/contacts/{contact.Id}", contact);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> EditContact(EditContactRequest req)
    {
        if (!MiniValidator.TryValidate(req, out var errors))
        {
            // return ValidationProblem(errors);
            return ValidationProblem();
        }

        var dbContact = await _contactRepository.GetContactByIdAsync(req.Id);
        if (dbContact == null)
        {
            return NotFound();
        }

        if (!Enum.TryParse<ContactType>(req.Category, true, out var parsedCategory))
        {
            // return ValidationProblem(new Dictionary<string, string[]>() { { "Category", new string[] { $"Invalid category: {req.Category}" } } });
            return ValidationProblem();
        }

        dbContact.DateOfBirth = req.DateOfBirth;
        dbContact.FirstName = req.FirstName;
        dbContact.LastName = req.LastName;
        dbContact.Email = req.Email;
        dbContact.TelephoneNumber = req.TelephoneNumber;
        dbContact.Category = parsedCategory;
        dbContact.SubCategory = req.SubCategory;

        await _contactRepository.SaveContactChangesAsync();
        return Ok(dbContact);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteContact(int id)
    {
        var contact = await _contactRepository.GetContactByIdAsync(id);
        if (contact == null)
        {
            return NotFound();
        }

        await _contactRepository.DeleteContactAsync(contact);
        return Ok();
    }
}