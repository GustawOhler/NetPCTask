using ContactList.Entities;

public interface IContactRepository
{
    public Task<IEnumerable<Contact>> GetAllContactsAsync();
    public Task<Contact?> GetContactByIdAsync(int id);
    public Task<Contact?> GetContactByEmailAsync(string email);
    public Task<Contact> InsertContactAsync(Contact contact);
    public Task SaveContactChangesAsync();
    public Task DeleteContactAsync(Contact contact);
}