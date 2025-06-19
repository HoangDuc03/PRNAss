using AutoMapper;
using BusinessObject.Models;
using DataAccess.Dto;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace eStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly HttpClient client = null;
        private string ApiUrl = "";
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public ProductController(IRepository<Category> categoryRepository, IConfiguration configuration, IMapper mapper, IRepository<Product> productRepository)
        {
            _categoryRepository = categoryRepository;
            _configuration = configuration;
            _mapper = mapper;
            _productRepository = productRepository;
        }
        [HttpGet("GetAll")]
        public async Task<List<Product>> GetAll()
        {
            return _productRepository.GetAll().ToList();
        }
        [HttpGet("GetId")]
        public async Task<Product> GetId(int id)
        {
            return _productRepository.GetId(id);
        }
        [HttpPut("Update")]
        public async Task<bool> Update(Product product)
        {
            try
            {
                if (product == null || product.ProductId == null)
                {
                    return false; 
                }
                var existingProduct = _productRepository.GetId((int)product.ProductId);
                if (existingProduct == null)
                {
                    return false; 
                }
                _mapper.Map(product, existingProduct);
                _productRepository.Update(existingProduct);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [HttpPost("Create")]
        public async Task<bool> Create(Product product)
        {
            try
            {
                if (product == null)
                {
                    return false; 
                }
                _productRepository.Add(product);
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
            _productRepository.Remove(id);
            return true;
        }
        [HttpGet("SearchByNameAndPrice")]
        public async Task<List<Product>> SearchByNameAndPrice(string name, decimal? price)
        {
            var query = _productRepository.GetAll().Where(p => p.ProductName.Contains(name) && (price == null || p.UnitPrice <= price)).ToList();
            return query;
        }
    }
}
