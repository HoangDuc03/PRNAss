using AutoMapper;
using BusinessObject.Models;
using DataAccess.Dto;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace eStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly HttpClient client = null;
        private string ApiUrl = "";
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public CategoryController(IRepository<Category> categoryRepository, IConfiguration configuration, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _configuration = configuration;
            _mapper = mapper;
        }
        [HttpGet("GetAll")]
        public async Task<List<Category>> GetAll()
        {
            return _categoryRepository.GetAll().ToList();
        }
        [HttpGet("GetId")]
        public async Task<Category> GetId(int id)
        {
            return _categoryRepository.GetId(id);
        }
        [HttpPut("Update")]
        public async Task<bool> Update(CategoryDto category)
        {
            try
            {
                if (category == null || category.CategoryId == null)
                {
                    return false; 
                }
                var existingCategory = _categoryRepository.GetId((int)category.CategoryId);
                if (existingCategory == null)
                {
                    return false;
                }
                _mapper.Map(category, existingCategory);
                _categoryRepository.Update(existingCategory);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [HttpPost("Create")]
        public async Task<bool> Create(Category category)
        {
            try
            {
                if (category == null)
                {
                    return false; 
                }
                _categoryRepository.Add(category);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [HttpGet("Delete")]
        public async Task<bool> Delete(int id)
        {
            try
            {
                _categoryRepository.Remove(id);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [HttpGet("GetByName")]
        public async Task<List<Category>> GetByName(string name)
        {
            var query = _categoryRepository.GetAll().Where(c => c.CategoryName.Contains(name)).ToList();
            return query;
        }
    }
}
