using ContactList.Entities;

public interface ICategoryRepository
{
    Task<Category?> GetCategoryByNameAsync(string name);
    Task<Subcategory?> GetSubcategoryByNameAsync(int categoryId, string name);
    Task<Subcategory> CreateSubcategoryAsync(int categoryId, string name, string visibleName);
    Task<IEnumerable<Category>> GetAllCategoriesWithSubcategoriesAsync();
}

