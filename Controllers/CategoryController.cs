using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data;
using RestaurantApp.DTOs;
using RestaurantApp.Models;

namespace RestaurantApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly DataContextEf _ef;
        public CategoryController(DataContextEf ef)
        {
            _ef = ef;
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            IEnumerable<Category> categories = await _ef.Categories.ToListAsync();
            return Ok(categories);
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory(CategoryDto categoryDto)
        {
            Category category = new Category
            {
                Name = categoryDto.Name
            };
            await _ef.Categories.AddAsync(category);
            await _ef.SaveChangesAsync();
            return Ok(category);
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryDto dto)
        {
            var category = await _ef.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            category.Name = dto.Name;
            _ef.Categories.Update(category);
            await _ef.SaveChangesAsync();
            return Ok(category);
        }

        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(int Id)
        {
            var category = await _ef.Categories.FirstOrDefaultAsync(c => c.CategoryId == Id);
            _ef.Categories.Remove(category);
            await _ef.SaveChangesAsync();
            return Ok(new { Response = "Category just got deleted", Category = category });
        }
    }
}
