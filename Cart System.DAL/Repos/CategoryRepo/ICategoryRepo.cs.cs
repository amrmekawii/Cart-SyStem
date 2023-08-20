namespace Cart_System.DAL;
public interface ICategoryRepo : IGenericRepo<Category>
{
    public IEnumerable<Product>? GetByIdWithProducts(int id);
    public IEnumerable<Category>? GetAllCategoriesWithAllProducts();

}
