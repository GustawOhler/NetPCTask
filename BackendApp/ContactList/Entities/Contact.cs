using System.ComponentModel.DataAnnotations;
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
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public int? SubcategoryId { get; set; }
    public Subcategory? Subcategory { get; set; }
    [MaxLength(30)]
    public string? TelephoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
