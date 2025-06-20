using BusinessObject.Models;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class GenericRepository<T> : IRepository<T> where T : class
{
    private static GenericRepository<T>? _instance;
    private static readonly object _lock = new object();
    private readonly EStoreContext _context;

    public GenericRepository(EStoreContext context)
    {
        _context = context;
    }

    public T Add(T entity)
    {
        _context.Set<T>().Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        _context.SaveChanges();
        return entity;
    }

    public T GetId(int id)
    {
        return _context.Set<T>().Find(id);
    }
    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().Where(predicate).ToList();
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public void Remove(int id)
    {
        var entity = GetId(id);
        _context.Remove(entity);
        _context.SaveChanges();
    }
    public IEnumerable<Order> GetOrdersByDateRange(DateOnly startDate, DateOnly endDate)
    {
        var ordersInDateRange = _context.Orders
                                        .Include(o => o.OrderDetails)
                                        .ThenInclude(od => od.Product)
                                        .Include(o => o.Member) 
                                        .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                                        .ToList(); 

        var sortedOrders = ordersInDateRange
            .Select(o => new {
                Order = o,
                TotalSales = o.OrderDetails.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount))
            })
            .OrderByDescending(o => o.TotalSales)
            .Select(o => o.Order);

        return sortedOrders.ToList();
    }
    public void RemoveMember(int memberId)
    {
        // Tìm thành viên cần xóa
        var member = _context.Members.Include(m => m.Orders) // Nạp các đơn hàng liên quan
                                     .ThenInclude(o => o.OrderDetails) // Nạp chi tiết đơn hàng
                                     .SingleOrDefault(m => m.MemberId == memberId);

        if (member != null)
        {
            // Xóa tất cả OrderDetails của các đơn hàng
            foreach (var order in member.Orders)
            {
                _context.OrderDetails.RemoveRange(order.OrderDetails);
            }

            // Xóa tất cả Orders
            _context.Orders.RemoveRange(member.Orders);

            // Cuối cùng, xóa Member
            _context.Members.Remove(member);

            _context.SaveChanges();
        }
    }
}
