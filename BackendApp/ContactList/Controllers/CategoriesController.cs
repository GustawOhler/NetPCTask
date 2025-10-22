using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    public CategoriesController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryRepository.GetAllCategoriesWithSubcategoriesAsync();
        return Ok(categories);
    }
}