using ContactList.DTOs;
using ContactList.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Thin controller to receive requests for Contacts and pass it to Service (handler) layer
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactsController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpGet]
    public async Task<IActionResult> GetContacts()
    {
        var contacts = await _contactService.GetContactsAsync();
        return Ok(contacts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetContactById(int id)
    {
        var result = await _contactService.GetContactByIdAsync(id);
        if (result.NotFound)
        {
            return NotFound();
        }

        return Ok(result.Value);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateContact(CreateContactRequest contactRequest)
    {
        var result = await _contactService.CreateContactAsync(contactRequest);
        if (!result.Success)
        {
            if (result.ValidationErrors != null)
            {
                return BadRequest(new
                {
                    title = "Validation error",
                    status = 400,
                    errors = result.ValidationErrors
                });
            }

            return Problem();
        }

        var createdContact = result.Value!;
        return Created($"/contacts/{createdContact.Id}", createdContact);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> EditContact(EditContactRequest req)
    {
        var result = await _contactService.UpdateContactAsync(req);
        if (result.NotFound)
        {
            return NotFound();
        }

        if (result.ValidationErrors != null)
        {
            return BadRequest(new
            {
                title = "Validation error",
                status = 400,
                errors = result.ValidationErrors
            });
        }

        return Ok(result.Value);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        var result = await _contactService.DeleteContactAsync(id);
        if (result.NotFound)
        {
            return NotFound();
        }

        if (!result.Success)
        {
            return BadRequest();
        }

        return Ok();
    }
}
