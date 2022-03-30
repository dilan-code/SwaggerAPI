using BibblanAPI.DTO;
using BibblanAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibblanAPI.AppDomain.Services.Interfaces
{
    public interface ICategoryService
    {
        ICollection<Category> GetCollectionOfCategories();
        bool CreateCategory(CategoryRequestDto categoryRequestDto);
        Category EditCategory(int id, CategoryRequestDto categoryRequestDto);
        bool DeleteCategory(int id);

    }
} 
