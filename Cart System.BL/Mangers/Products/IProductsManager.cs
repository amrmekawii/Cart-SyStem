using Cart_System.DAL;

namespace Cart_System.BL;

public interface IProductsManager
{
   public ProductDetailsDto GetProductByID(int id);



    IEnumerable<ProductReadDto> GetAllProducts();
    public bool AddProduct(ProductAddDto productDto);
    bool EditProduct(ProductEditDto productEditDto);
    bool DeleteProduct(int Id);




}
