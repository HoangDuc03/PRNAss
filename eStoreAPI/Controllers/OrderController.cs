using AutoMapper;
using BusinessObject.Models;
using DataAccess.Dto;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace eStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderDetail> _orderdetailRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public OrderController(IRepository<Order> orderRepository,IRepository<OrderDetail> orderdetailrepository, IConfiguration configuration, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderdetailRepository = orderdetailrepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<List<Order>> GetAll()
        {
            return _orderRepository.GetAll().ToList();
        }
        [HttpGet("GetId")]
        public async Task<Order> GetId(int id)
        {
            return _orderRepository.GetId(id);
        }
        [HttpPut("Update")]
        public async Task<bool> Update(OrderDto order)
        {
            try
            {
                if (order == null || order.OrderId == null)
                {
                    return false; 
                }
                var existingOrder = _orderRepository.GetId((int)order.OrderId);
                if (existingOrder == null)
                {
                    return false; 
                }
                _mapper.Map(order, existingOrder);
                _orderRepository.Update(existingOrder);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [HttpPost("Create")]
        public async Task<bool> Create(OrderDto order)
        {
            try
            {
                if (order == null)
                {
                    return false; 
                }
                else
                {
                    var newOrder = _mapper.Map<Order>(order);
                    _orderRepository.Add(newOrder);
                }
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
                var order = _orderRepository.GetId(id);
                if (order == null)
                {
                    return false;
                }
                _orderRepository.Remove(id);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [HttpGet("Report")]
        public IActionResult GetSalesReport(DateOnly startDate, DateOnly endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start Date cannot be after End Date.");
            }

            try
            {
                var orders = _orderRepository.GetOrdersByDateRange(startDate, endDate);
                if (orders == null || !orders.Any())
                {
                    return NotFound("No orders found in the given date range.");
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
