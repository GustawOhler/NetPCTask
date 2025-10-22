using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ContactList.Entities;

[Index(nameof(Name), IsUnique = true)]
public class Category
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty; // string key

    [Required, MaxLength(100)]
    public string VisibleName { get; set; } = string.Empty;

    public ICollection<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
}

