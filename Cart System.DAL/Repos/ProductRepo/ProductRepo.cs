using Microsoft.EntityFrameworkCore;

namespace Cart_System.DAL;
public class ProductRepo : GenericRepo<Product>, IProductRepo
{
    private readonly CartSystemContext _context;

    public ProductRepo(CartSystemContext context) : base(context)
    {
        _context = context;
    }


    public Product? GetProductByIdWithCategory(int id)
    {
        return _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.ProductImages)
                    .FirstOrDefault(p => p.Id == id);
    }


    #region Get Product Details with images
    public Product? GetProductByIdWithimages(int id)
    {
        return _context.Set<Product>()
                    .Include(p => p.ProductImages)
                    .FirstOrDefault(p => p.Id == id);
    }

    #endregion


    #region Get all With Category

    public IEnumerable<Product> GetAllWithCategory()
    {
        return _context.Set<Product>()
                       .Include(x => x.Category)
                       .Include(p => p.ProductImages);

    }
    #endregion



 
    public int GetCount()
    {
        return _context.Products.Count();
    }







   
}