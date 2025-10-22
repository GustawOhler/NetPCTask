using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ContactList.Entities;

[Index(nameof(Name), nameof(CategoryId), IsUnique = true)]
public class Subcategory
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty; // string key

    [Required, MaxLength(100)]
    public string VisibleName { get; set; } = string.Empty;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}

