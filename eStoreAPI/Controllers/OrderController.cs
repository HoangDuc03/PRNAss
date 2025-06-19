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
                    return false; // Or throw an exception based on your logic
                }
                var existingOrder = _orderRepository.GetId((int)order.OrderId);
                if (existingOrder == null)
                {
                    return false; // Or throw an exception based on your logic
                }
                _mapper.Map(order, existingOrder);
                _orderRepository.Update(existingOrder);
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
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
                    return false; // Or throw an exception based on your logic
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
                // Log the exception or handle it as needed
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
                    return false; // Or throw an exception based on your logic
                }
                _orderRepository.Remove(id);
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return false;
            }
        }
    }
}
