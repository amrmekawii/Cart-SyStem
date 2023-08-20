
using Cart_System.DAL;
using Microsoft.AspNetCore.Components;

namespace Cart_System.BL;

public class CategoriesManager: ICategoriesManager
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoriesManager(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    #region GetAllCategories
    public IEnumerable<CategoryDto> GetAllCategoriesDto()
    {
        IEnumerable<Category> categoriesFromDb = _unitOfWork.CategoryRepo.GetAll();
        IEnumerable<CategoryDto> categoriesDto = categoriesFromDb.Select(c => new CategoryDto()
        {
            Name = c.Name,
            Id = c.Id,
        });
        return categoriesDto;
    }


    #endregion


    #region GetCategoryById
    public CategoryDto? GetCategoryById(int id)
    {
        Category? category = _unitOfWork.CategoryRepo.GetById(id);
        if (category is null) { return null; }
        return new CategoryDto()
        {
            Id = category.Id,
            Name = category.Name,

        };

    }
    #endregion


    #region Get Category with Products By Id
    public IEnumerable<ProductChildDto>? GetCategoryWithProducts(int id)
    {
        IEnumerable<Product>? ProductsFromDb = _unitOfWork.CategoryRepo.GetByIdWithProducts(id);
        if (ProductsFromDb is null) { return null; };

        var productsInThisCategory = ProductsFromDb.Select(p => new ProductChildDto
        {

            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Discount= p.Discount,


        });

        return productsInThisCategory;
        
    }

    #endregion


    #region Get All Categories with products
    public IEnumerable<CategoryDetailsDto> GetAllCategoriesWithProducts()
    {
        IEnumerable<Category>? categoriesFromDb = _unitOfWork.CategoryRepo.GetAllCategoriesWithAllProducts();
        IEnumerable<CategoryDetailsDto> CategoriesDto = categoriesFromDb
            .Select(c => new CategoryDetailsDto
            {
                Id = c.Id,
                Name = c.Name,
                Products = c.Products.Select(p => new ProductChildDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Discount= p.Discount,

                }).ToList()

            });
        return CategoriesDto;

    }
    #endregion

    #region Add Category

    public bool AddCategory(CategoryAddDto category)
    {
        Category CategoryToAdd = new Category
        {
            Name = category.Name,
        };

        _unitOfWork.CategoryRepo.Add(CategoryToAdd);
        return _unitOfWork.Savechanges() > 0;
    }

    #endregion

    #region Update Category

    public bool UpdateCategory(CategoryEditDto categoryEditDto)
    {
        var category = _unitOfWork.CategoryRepo.GetById(categoryEditDto.Id);
        if (category is null)
        {
            return false;
        }

        category.Name = categoryEditDto.Name;

        return _unitOfWork.Savechanges() > 0;
    }

    #endregion

    #region Delete Category

    public bool DeleteCategory(int Id)
    {
        var category = _unitOfWork.CategoryRepo.GetById(Id);
        if (category is null)
        {
            return false;
        }

        _unitOfWork.CategoryRepo.Delete(category);
        return _unitOfWork.Savechanges() > 0;
    }

    #endregion

}









