using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using AutoMapper;
using BibblanAPI.AppDomain.Services.Interfaces;
using BibblanAPI.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BibblanAPI.Controller
{
    [ApiController]
    [Route("v1/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        // GET: Get collection of categories
        [HttpGet]
        [ProducesResponseType(typeof(List<CategoryResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult GetCollectionOfCategories()
        {
            var collection = _categoryService.GetCollectionOfCategories();
            if (!collection.Any())
            {
                return NoContent();
            }
            var result = _mapper.Map<List<CategoryResponseDto>>(collection);
            return Ok(result);

        }

        // POST: Create Category
        /// <param name="categoryRequestDto"></param>
        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult CreateCategory([FromBody] CategoryRequestDto categoryRequestDto)
        {
            var category = _categoryService.CreateCategory(categoryRequestDto);

            if (!category)
            {
                return UnprocessableEntity();
            }
            return Ok(category);
        }


        // PUT: Edit Category
        /// <param name="categoryId"></param>
        /// <param name="categoryRequestDto"></param>
        [HttpPut("{categoryId}")]
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult EditCategory([FromRoute] int categoryId, [FromBody] CategoryRequestDto categoryRequestDto)
        {
            var edit = _categoryService.EditCategory(categoryId, categoryRequestDto);
            if (edit == null)
            {
                return UnprocessableEntity();
            }
            var result = _mapper.Map<CategoryResponseDto>(edit);
            return Ok(result);
        }


        // DELETE: Delete Category
        /// <param name="categoryId"></param>
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult DeleteCategory([FromRoute] int categoryId)
        {
            var delete = _categoryService.DeleteCategory(categoryId);
            if (!delete)
            {
                return UnprocessableEntity();
            }
            return Ok(delete);
        }


    }
}