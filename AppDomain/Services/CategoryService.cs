using BibblanAPI.AppDomain.Services.Interfaces;
using BibblanAPI.DTO;
using BibblanAPI.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace BibblanAPI.AppDomain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly DbBibblanContext _dbBibblanContext;

        public CategoryService(DbBibblanContext dbBibblanContext)
        {
            _dbBibblanContext = dbBibblanContext;
        }


        public bool CreateCategory(CategoryRequestDto categoryRequestDto)
        {
            if (DuplicateCategory(categoryRequestDto.CategoryName))
            {
                return false;
            }

            var newCategory = new Category();
            newCategory.CategoryName = categoryRequestDto.CategoryName;
            _dbBibblanContext.Add(newCategory);

            if (_dbBibblanContext.SaveChanges() == 1)
            {
                return true;
            }
            return false;

        }

        public bool DeleteCategory(int id)
        {
            var category = _dbBibblanContext.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return false; // Kategorin hittades inte
            }

            var libraryItem = _dbBibblanContext.LibraryItems.FirstOrDefault(x => x.CategoryId == id);

            if (libraryItem != null)
            {
                return false; // Det finns en artikel som refererar till kategorin som vill tas bort
            }

            _dbBibblanContext.Categories.Remove(category);

            if (_dbBibblanContext.SaveChanges() == 1)
            {
                return true;
            }
            return false;
        }

        public Category EditCategory(int id, CategoryRequestDto categoryRequestDto)
        {
            var category = _dbBibblanContext.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return null; // Kategorin hittades inte
            }

            category.CategoryName = categoryRequestDto.CategoryName;
            if (_dbBibblanContext.SaveChanges() == 1)
            {
                return category;
            }
            return null;
        }

        public ICollection<Category> GetCollectionOfCategories()
        {
            var categoryCollection = _dbBibblanContext.Categories.ToList();
            return categoryCollection;
        }

        private bool DuplicateCategory(string categoryName) // Kontrollerar att kategori namn inte dyker upp två ggr
        {
            var category = _dbBibblanContext.Categories.FirstOrDefault(x => x.CategoryName == categoryName);

            if (category != null)
            {
                return true;
            }

            return false;
        }

    }
}
