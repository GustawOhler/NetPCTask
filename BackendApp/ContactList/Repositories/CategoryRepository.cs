using ContactList.Entities;
using Microsoft.EntityFrameworkCore;

public class CategoryRepository : ICategoryRepository
{
    private readonly DbContext _context;

    public CategoryRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetCategoryByNameAsync(string name)
    {
        var key = name.Trim().ToLower();
        return await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == key);
    }

    public async Task<Subcategory?> GetSubcategoryByNameAsync(int categoryId, string name)
    {
        var key = name.Trim().ToLower();
        return await _context.Subcategories.FirstOrDefaultAsync(s => s.CategoryId == categoryId && s.Name.ToLower() == key);
    }

    public async Task<Subcategory> CreateSubcategoryAsync(int categoryId, string name, string visibleName)
    {
        var sub = new Subcategory
        {
            CategoryId = categoryId,
            Name = name.Trim(),
            VisibleName = visibleName.Trim()
        };
        _context.Subcategories.Add(sub);
        await _context.SaveChangesAsync();
        return sub;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesWithSubcategoriesAsync()
    {
        return await _context.Categories
            .Include(c => c.Subcategories)
            .ToListAsync();
    }
}

