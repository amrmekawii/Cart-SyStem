using Microsoft.EntityFrameworkCore;

namespace Cart_System.DAL;
public class CategoryRepo : GenericRepo<Category>, ICategoryRepo
{
    private readonly CartSystemContext _context;

    public CategoryRepo(CartSystemContext context) : base(context)
    {
        _context = context;

    }

    #region Get Category By ID  With  Products
    public IEnumerable<Product>? GetByIdWithProducts(int id)
    {
        return _context.Products
               .Include(p => p.Category)
               .Where(c => c.CategoryID == id).ToList();

    }
    #endregion

    #region Get All Categories With all products
    public IEnumerable<Category>? GetAllCategoriesWithAllProducts()
    {
        return _context.Categories
              .Include(c => c.Products);
    }
    #endregion
}













