using System.ComponentModel.DataAnnotations;
using ContactList.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ContactList.Entities;

[Index(nameof(Email), IsUnique = true)]
public class Contact
{
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string FirstName { get; set; } = "";
    [Required, MaxLength(100)]
    public string LastName { get; set; } = "";
    [Required, MaxLength(512)]
    public string Email { get; set; } = "";
    public ContactType Category { get; set; } = ContactType.Other;
    public string? SubCategory { get; set; }
    [MaxLength(30)]
    public string? TelephoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
}