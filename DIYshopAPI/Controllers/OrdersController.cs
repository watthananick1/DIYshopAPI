using System.Globalization;
using System.Text;
using DIYshopAPI.Data;
using DIYshopAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace DIYshopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderContext _context;
        private readonly ProductContext _productContext;
        public OrdersController(OrderContext context, ProductContext productContext)
        {
            _context = context;
            _productContext = productContext;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var customer = await _context.Orders.ToListAsync();
            return customer == null ? BadRequest("Order Not Found.") : Ok(customer);
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Orders.FindAsync(id);
            return customer == null ? BadRequest("Order Not Found.") : Ok(customer);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, OrderUpdate orderUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var order = await _context.Orders.FindAsync(id);
            var dataOrder = order;
            if (order == null) return BadRequest();

            order.Date = (DateTime)orderUpdate.Date;
            order.Total_Price = orderUpdate.Total_Price ?? dataOrder.Total_Price;
            order.User_Id = orderUpdate.User_Id ?? dataOrder.User_Id;
            order.Customer_Id = orderUpdate.Customer_Id ?? dataOrder.Customer_Id;
            order.Promotion_id = orderUpdate.Promotion_id ?? dataOrder.Promotion_id;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _context.Orders.FindAsync(id);
            if (customer == null) return NotFound();

            _context.Orders.Remove(customer);
            await _context.SaveChangesAsync();
            return NoContent();

        }

        [HttpPost("listOrderSellpdf")]
        public async Task<IActionResult> GeneratePDF(List<OrderItem> listOrderItem)
        {
            // Check for null listOrderItem
            Console.WriteLine(listOrderItem);
            
            if (listOrderItem == null || listOrderItem.Count == 0)
            {
                return BadRequest();
            }

            var document = new PdfDocument();
            var order = new Order();
            var Date = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

            // Generate HTML content
            string htmlContent = await GeneratehtmlContentAsync(listOrderItem, order, Date, Get_productContext());

            // Add the HTML content to the PDF document
            PdfGenerator.AddPdfPages(document, htmlContent, PageSize.A4);

            // Convert the PDF document to a byte array
            byte[] pdfBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                pdfBytes = ms.ToArray();
            }

            // Set the filename for the PDF
            string filename = "Order.pdf";

            // Return the PDF as a file for download
            return File(pdfBytes, "application/pdf", filename);
        }

        private ProductContext Get_productContext()
        {
            return _productContext;
        }

        private async Task<string> GeneratehtmlContentAsync(List<OrderItem> listOrderItem, Order order, string date, ProductContext _productContext)
        {
            // HTML header
            string htmlContent = "<div style='width:100%; text-align:left'>";
            htmlContent += "<h1>ร้าน DIY Shop</h1>";

            htmlContent += "<h1 style='border:1px solid #000'>ใบเสร็จรับเงิน</h1>";
            htmlContent += "<p>เมื่อ " + date + "</p>";
            htmlContent += "</div>";

            // Table header
            htmlContent += "<table style='width:100%; border: 1px solid #000'>";
            htmlContent += "<thead style='font-weight:bold'>";
            htmlContent += "<tr>";
            htmlContent += "<td style='border:1px solid #000'> ลำดับ </td>";
            htmlContent += "<td style='border:1px solid #000'> รหัสสินค้า</td>";
            htmlContent += "<td style='border:1px solid #000'>ชื่อสินค้า</td>";
            htmlContent += "<td style='border:1px solid #000'>จำนวน</td>";
            htmlContent += "<td style='border:1px solid #000'>ราคา(หน่วย/บาท)</td>";
            htmlContent += "<td style='border:1px solid #000'>ราคารวม</td>";
            htmlContent += "</tr>";
            htmlContent += "</thead>";

            // Table body
            htmlContent += "<tbody>";
            decimal totalOrderPrice = 0;
            int countProduct = 0;
            int number = 0;

            foreach (var item in listOrderItem)
            {
                number++;
                decimal totalItemPrice = item.Item_Price * (decimal)item.Item_Quantity;
                var product = await _productContext.Products.FindAsync(item.Product_id);

                htmlContent += "<tr>";
                htmlContent += "<td style='text-align:left'>" + number + "</td>";
                htmlContent += "<td>" + item.N_Id + "</td>";

                htmlContent += "<td>" + product.Name + "</td>";
                htmlContent += "<td>" + item.Item_Quantity + "</td>";
                htmlContent += "<td>" + string.Format("{0:#,0.00}", item.Item_Price) + "</td>";
                htmlContent += "<td>" + string.Format("{0:#,0.00}", totalItemPrice) + "</td>";
                htmlContent += "</tr>";

                totalOrderPrice += totalItemPrice;
                countProduct += item.Item_Quantity;
            }
            htmlContent += "</tbody>";

            // Table footer

            htmlContent += "</table>";

            htmlContent += "<div style='text-align:left'>";
            htmlContent += "<table style='float:left' >";
            htmlContent += "<tr>";
            htmlContent += "<td > จำนวนสินค้าทั้งหมด " + countProduct + " รายการ </td>";
            htmlContent += "</tr>";

            htmlContent += "</table>";
            htmlContent += "</div>";

            htmlContent += "<div style='text-align:right'>";
            htmlContent += "<table style='font-weight:bold; float:left' >";
            htmlContent += "<tr>";
            htmlContent += "<td style='font-size: 16px; border:1px solid #000; font-weight:bold'> ราคาสินค้าสุทธิ " + string.Format("{0:#,0.00, บาท}", totalOrderPrice) + "</td>";
            htmlContent += "</tr>";

            htmlContent += "</table>";
            htmlContent += "</div>";

            htmlContent += "</div>";


            return htmlContent.ToString();
        }
    }
}
