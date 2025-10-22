using Microsoft.EntityFrameworkCore;
using ContactList.Entities;

public class ContactRepository : IContactRepository
{
    private DbContext _context;

    public ContactRepository(DbContext context)
    {
        _context = context;
    }

    public async Task DeleteContactAsync(Contact contact)
    {
        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();
    }

    public async Task SaveContactChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Contact>> GetAllContactsAsync()
    {
        return await _context.Contacts
            .Include(c => c.Category)
            .Include(c => c.Subcategory)
            .ToListAsync();
    }

    public async Task<Contact?> GetContactByIdAsync(int id)
    {
        return await _context.Contacts
            .Include(c => c.Category)
            .Include(c => c.Subcategory)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Contact> InsertContactAsync(Contact contact)
    {
        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();
        return contact;
    }

    public async Task<Contact?> GetContactByEmailAsync(string email)
    {
        return await _context.Contacts.FirstOrDefaultAsync(x => x.Email == email);
    }
}
