namespace Cart_System.DAL;
public interface IProductRepo :IGenericRepo<Product>
{
    public Product? GetProductByIdWithCategory(int id);
    Product? GetProductByIdWithimages(int id);

    IEnumerable<Product> GetAllWithCategory();
    int GetCount();



}
