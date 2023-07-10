using DIYshopAPI.Data;
using DIYshopAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Globalization;

namespace DIYshopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly OrderItemContext _orderItemContext;
        private readonly ProductContext _productContext;
        private readonly UserdbContext _userContext;
        private readonly OrderContext _orderContext;
        private readonly CustomerContext _customerContext;
        public SearchController(OrderItemContext orderItemContext, ProductContext productContext, UserdbContext userContext, OrderContext orderContext, CustomerContext customerContext)
        {
            _orderItemContext = orderItemContext;
            _productContext = productContext;
            _userContext = userContext;
            _orderContext = orderContext;
            _customerContext = customerContext;
        }

        [HttpPost("searchStock")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchStock(SalesReportItem parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = _productContext.Products.AsQueryable();

            // Filter by product type
            if (parameters.Type.HasValue)
            {
                query = query.Where(item => item.Type == parameters.Type);
            }

            var products = await query.ToListAsync();

            // Group by product category
            var stockReport = await query
                .GroupBy(item => item.Type)
                .Select(g => new SalesReportItem
                {
                    Type = parameters.Type,
                    TotalQuantity = g.Sum(item => item.Stock),
                    TotalPrice = g.Sum(item => item.Stock * item.Price)
                })
                .ToListAsync();

            var result = new
            {
                Products = products,
                StockReport = stockReport
            };

            return Ok(result);
        }

        [HttpPost("searchSales")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchSales(SalesReportParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IQueryable<Order> query = _orderContext.Orders.AsQueryable();
            IQueryable<OrderItem> queryItem = _orderItemContext.OrderItems.AsQueryable();
            IQueryable<Product> queryproduct = _productContext.Products.AsQueryable();

            List<Order> bindingSource2 = new List<Order>();
            List<OrderItem> filteredOrderItems = new List<OrderItem>();

            // Filter by product type
            if (parameters.Type.HasValue)
            {
                List<int> productIds = await queryproduct
                    .Where(p => p.Type == parameters.Type)
                    .Select(p => p.Id).ToListAsync();

                filteredOrderItems = await queryItem
                    .Where(item => productIds.Contains(item.Product_id))
                    .ToListAsync();

            }


            // Filter by date range
            if (parameters.StartDate.HasValue && parameters.EndDate.HasValue)
            {
                string DateFormat2 = "yyyy-MM-dd";
                var ci = new CultureInfo("th-TH");
                string d1 = " 00:00:00";
                string d2 = " 23:59:59";
                var startDate = Convert.ToDateTime(parameters.StartDate.Value.Date.ToString(DateFormat2, ci) + d1);
                var endDate = Convert.ToDateTime(parameters.EndDate.Value.Date.ToString(DateFormat2, ci) + d2);

                //Console.WriteLine(startDate + " " + endDate);

                var results = await query.ToListAsync();
                foreach (var item in results)
                {
                    string eh;
                    var dt = Convert.ToDateTime(item.Date.ToString(DateFormat2, ci) + d1);
                    //Console.WriteLine("** {0}", dt);
                    var td1 = Convert.ToDateTime(startDate.ToString(DateFormat2, ci) + d1);
                    var td1end = Convert.ToDateTime(endDate.ToString(DateFormat2, ci) + d2);
                    //Console.WriteLine("-*- {0}", startDate);
                    //Console.WriteLine("-- {0}", endDate);
                    if (item.Date >= startDate || dt >= td1)
                    {
                        if (item.Date < endDate || dt < td1end)
                        {
                            bindingSource2.Add(item);
                        }

                    }
                }
            }

            // Filter by duration (number of days)
            if (parameters.Duration.HasValue)
            {
                var startDate = DateTime.Now.AddDays(-parameters.Duration.Value);
                query = query.OrderByDescending(item => item.Date >= startDate);
                var item = await query.ToListAsync();
                bindingSource2 = item;
            }

            // Filter by latest number of days
            if (parameters.LatestDays.HasValue)
            {
                var startDate = DateTime.Now.AddDays(-parameters.LatestDays.Value);
                query = query.OrderByDescending(item => item.Date >= startDate);
                var item = await query.ToListAsync();
                bindingSource2 = item;
            }

            // Filter by month
            if (parameters.Month.HasValue)
            {
                int monthValue = parameters.Month.Value;
                query = query.Where(item => item.Date.Month == monthValue)
                             .OrderByDescending(item => item.Date);
                var item = await query.ToListAsync();
                bindingSource2 = item;

            }

            // Group by product category
            var salesReport = await queryItem
                .GroupBy(item => item.Product_id)
                .Select(g => new SalesReportItem
                {
                    Type = (ProductType?)g.Key,
                    TotalQuantity = g.Sum(item => item.Item_Quantity),
                    TotalPrice = g.Sum(item => item.Item_Quantity * item.Item_Price)
                })
                .ToListAsync();
            var result = new
            {
                OrderItem = filteredOrderItems,
                Order = bindingSource2,
                SalesReport = salesReport
            };
            return Ok(result);
        }

        [HttpPost("searchPurchases")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchPurchases(CustomerPurchaseParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IQueryable<Order> query = _orderContext.Orders.AsQueryable();
            IQueryable<OrderItem> queryItem = _orderItemContext.OrderItems.AsQueryable();
            IQueryable<Customer> queryCustomer = _customerContext.Customers.AsQueryable();

            // Filter by customer name or customer ID
            if (!string.IsNullOrEmpty(parameters.CustomerName))
            {
                var customerIds = await queryCustomer
                    .Where(item => item.Firstname.Contains(parameters.CustomerName) || item.Lastname.Contains(parameters.CustomerName))
                    .Select(item => item.Id)
                    .FirstOrDefaultAsync();
                query = query.Where(item => item.Customer_Id == customerIds);
                var id = await query.Where(item => item.Customer_Id == customerIds).Select(item => item.Id).FirstOrDefaultAsync();
                queryItem = queryItem.Where(item => item.Order_Id == id);
            }

            if (parameters.CustomerId.HasValue)
            {
                queryCustomer = queryCustomer.Where(item => item.Id == parameters.CustomerId.Value);
                query = query.Where(item => item.Customer_Id == parameters.CustomerId.Value);
                var id = await query.Where(item => item.Customer_Id == parameters.CustomerId.Value).Select(item => item.Id).FirstOrDefaultAsync();
                queryItem = queryItem.Where(item => item.Order_Id == id);
            }


            var orders = await query.ToListAsync();
            var orderItems = await queryItem.ToListAsync();
            var orderCustomers = await queryCustomer.ToListAsync();

            // Group by customer and calculate total purchase amount
            var customerPurchases = await queryItem
                .GroupBy(item => item.Id)
                .Select(g => new CustomerPurchase
                {
                    CustomerId = g.Key,
                    CustomerName = parameters.CustomerName,
                    TotalPurchaseAmount = g.Sum(item => item.Item_Quantity),
                    AmountPaid = g.Sum(item => item.Item_Quantity * item.Item_Price)
                })
                .ToListAsync();

            var result = new
            {
                Order = orders,
                OrderItem = orderItems,
                OrderCustomer = orderCustomers,
                CustomerPurchases = customerPurchases
            };
            return Ok(result);
        }






    }
}
