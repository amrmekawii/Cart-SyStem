
namespace Cart_System.BL;

public interface ICategoriesManager
{
    IEnumerable<CategoryDto> GetAllCategoriesDto();
    public CategoryDto? GetCategoryById(int id);

    IEnumerable<ProductChildDto>? GetCategoryWithProducts(int id);
    IEnumerable<CategoryDetailsDto> GetAllCategoriesWithProducts();

    bool AddCategory(CategoryAddDto category);
    bool UpdateCategory(CategoryEditDto categoryEditDto);
    bool DeleteCategory(int Id);
}









