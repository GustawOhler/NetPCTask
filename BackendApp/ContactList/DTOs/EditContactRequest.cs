using System.ComponentModel.DataAnnotations;

namespace ContactList.DTOs;

public class EditContactRequest : CreateContactRequest
{
    [Required, Range(1, int.MaxValue)]
    public int Id { get; set; }
}