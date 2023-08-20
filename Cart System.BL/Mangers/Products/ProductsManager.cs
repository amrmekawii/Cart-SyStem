

using Cart_System.DAL;

using Microsoft.AspNetCore.Mvc;

namespace Cart_System.BL;

public class ProductsManager: IProductsManager
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductsManager(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    #region Get All Products in Database
    // public IEnumerable<ProductChildDto> GetAllProductsWithAvgRating()
    // {
    //     IEnumerable<Product> productsFromDb = _unitOfWork.ProductRepo.GetAllProductsWithAvgRating();
    //     IEnumerable<ProductChildDto> productsDtos = productsFromDb
    //         .Select(p => new ProductChildDto
    //         {
    //             Id = p.Id,
    //             Name = p.Name,
    //             Price = p.Price,
    //             Image = p.ProductImages.FirstOrDefault()?.ImageUrl??"",
    //             Discount = p.Discount,
    //             AvgRating = p.Reviews.Any() ? (decimal)p.Reviews.Average(r => r.Rating) : 0,
    //             ReviewCount=p.Reviews.Count()

    //         });
    //     return productsDtos;
    // }

    ////version before making pagination

    #endregion


    #region Get Product Details
    public ProductDetailsDto? GetProductByID(int id)
    {
        Product? productFromDb = _unitOfWork.ProductRepo.GetProductByIdWithCategory(id);

        if (productFromDb is null) { return null; }
        return new ProductDetailsDto()
        {
            Id = productFromDb.Id,
            Name = productFromDb.Name,
            Price = productFromDb.Price,
            Discount = productFromDb.Discount,
            Description = productFromDb.Description,
            Model = productFromDb.Model,
            CategoryId = productFromDb.CategoryID,
            CategoryName = productFromDb.Category.Name,
            Images = productFromDb.ProductImages.Select(i => i.ImageUrl).ToList(),

          


        };
    }
    #endregion     

    

    #region Get All Products

    public IEnumerable<ProductReadDto> GetAllProducts()
    {
        IEnumerable<Product> productsFromDb = _unitOfWork.ProductRepo.GetAllWithCategory();
        if (productsFromDb is null)
        {
            return null;
        }

        IEnumerable<ProductReadDto> productReadDto = productsFromDb
            .Select(p => new ProductReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryName = p.Category.Name,
                Image = p.ProductImages.FirstOrDefault()?.ImageUrl??"",

            });
        return productReadDto;
    }

    #endregion

    #region Add Product
    public bool AddProduct(ProductAddDto productDto)
    {
        var ProductToAdd = new Product
        {
            Name = productDto.Name,
            Price = productDto.Price,
            CategoryID = productDto.CategoryID,
            Description = productDto.Description,
            Model = productDto.Model,
            Discount = productDto.Discount,
            ProductImages = productDto.Image.Select(i => new ProductImages
            {
                ImageUrl = i
            }).ToList()

        };
        _unitOfWork.ProductRepo.Add(ProductToAdd);
        return _unitOfWork.Savechanges() > 0;
    }

    #endregion

    #region Edit Product

    public bool EditProduct(ProductEditDto productEditDto)
    {
        var product = _unitOfWork.ProductRepo.GetProductByIdWithimages(productEditDto.Id);
        if (product is null)
        {
            return false;
        }

        product.Name = productEditDto.Name;
        product.Price = productEditDto.Price;
        product.Description = productEditDto.Description;
        product.ProductImages = productEditDto.ImagesURLs.Select(i => new ProductImages
        {
            ImageUrl = i
        }).ToList();
        product.Model = productEditDto.Model;
        product.CategoryID = productEditDto.CategoryID;
        product.Discount = productEditDto.Discount;

        return _unitOfWork.Savechanges() > 0;
    }

    #endregion

    #region Delete Product

    public bool DeleteProduct(int Id)
    {
        var product = _unitOfWork.ProductRepo.GetById(Id);
        if (product is null)
        {
            return false;
        }

        _unitOfWork.ProductRepo.Delete(product);
        return _unitOfWork.Savechanges() > 0;
    }

    #endregion

    




}











