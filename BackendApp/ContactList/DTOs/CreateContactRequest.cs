using System.ComponentModel.DataAnnotations;

namespace ContactList.DTOs;

public class CreateContactRequest
{
    [Required, MaxLength(100)]
    public string FirstName { get; set; } = "";
    [Required, MaxLength(100)]
    public string LastName { get; set; } = "";
    [Required, EmailAddress, MaxLength(512)]
    public string Email { get; set; } = "";
    [Required]
    public string Category { get; set; } = "";
    public string? SubCategory { get; set; }
    [Phone]
    public string? TelephoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
