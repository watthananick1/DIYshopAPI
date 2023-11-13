using System.Reflection.PortableExecutable;
using DIYshopAPI.Data;
using DIYshopAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace DIYshopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductContext _context;
        public ProductController(ProductContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var products = await _context.Products.ToListAsync();
            return products == null ? BadRequest("Product Not Found.") : Ok(products);
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

            var product = await _context.Products.FindAsync(id);
            return product == null ? BadRequest("Product Not Found.") : Ok(product);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, ProductUpdate productUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = await _context.Products.FindAsync(id);
            var dataCustomer = product;
            if (product == null) return BadRequest();

            product.N_Id = productUpdate.N_Id ?? dataCustomer.N_Id;
            product.Name = productUpdate.Name ?? dataCustomer.Name;
            product.Description = productUpdate.Description ?? dataCustomer.Description;
            product.Price = productUpdate.Price ?? dataCustomer.Price;
            product.Stock = productUpdate.Stock ?? dataCustomer.Stock;
            product.Type = productUpdate.Type ?? dataCustomer.Type;
            product.ImgPoduct = productUpdate.ImgPoduct ?? dataCustomer.ImgPoduct;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _context.Products.FindAsync(id);
            if (customer == null) return NotFound();

            _context.Products.Remove(customer);
            await _context.SaveChangesAsync();
            return NoContent();

        }

        [HttpPost("listProductpdf")]
        public async Task<IActionResult> GeneratePDF(int[] listproduct)
        {
            var document = new PdfDocument();
            List<Product> products = new List<Product>();

            foreach (int item in listproduct)
            {
                var product = await _context.Products.FindAsync(item);
                if (product != null)
                {
                    Console.WriteLine(product);
                    products.Add(product);
                }
            }

            string htmlcontent = "<div style='width:100%; text-align:center'>";
            htmlcontent += "<h2>ร้าน DIY Shop</h2>";
            htmlcontent += "</div>";

            htmlcontent += "<div style='text-align:left'>";
            htmlcontent += "<h1> รายการสินค้า </h1>";

            htmlcontent += "<table style ='width:100%; border: 1px solid #000'>";
            htmlcontent += "<thead style='font-weight:bold'>";
            htmlcontent += "<tr>";
            htmlcontent += "<td style='border:1px solid #000'> ลำดับ </td>";
            htmlcontent += "<td style='border:1px solid #000'> รหัสสินค้า </td>";
            htmlcontent += "<td style='border:1px solid #000'> ชื่อสินค้า </td>";
            htmlcontent += "<td style='border:1px solid #000'> จำนวนคงเหลือ </td>";
            htmlcontent += "<td style='border:1px solid #000'> ราคา(หน่วย/บาท) </td >";
            htmlcontent += "</tr>";
            htmlcontent += "</thead >";

            htmlcontent += "<tbody>";
            var number = 0;
            if (products != null && products.Count > 0)
            {
                products.ForEach(item =>
                {
                    number += 1;
                    htmlcontent += "<tr>";
                    htmlcontent += "<td style='text-align:center'>" + number + "</td>";
                    htmlcontent += "<td>" + item.N_Id + "</td>";
                    htmlcontent += "<td>" + item.Name + "</td>";
                    htmlcontent += "<td>" + item.Stock + "</td >";
                    htmlcontent += "<td>" + item.Price + "</td>";
                    htmlcontent += "</tr>";
                });
            }
            htmlcontent += "</tbody>";

            htmlcontent += "</table>";

            htmlcontent += "<div style='text-align:left'>";
            htmlcontent += "<table style='float:left' >";
            htmlcontent += "<tr>";
            htmlcontent += "<td > จำนวนสินค้าทั้งหมด " + products.Count + " รายการ </td>";
            htmlcontent += "</tr>";

            htmlcontent += "</table>";
            htmlcontent += "</div>";

            htmlcontent += "</div>";

            PdfGenerator.AddPdfPages(document, htmlcontent, PageSize.A4);
            //}
            byte[]? response = null;
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                response = ms.ToArray();
            }
            string Filename = "ListProduct" + ".pdf";
            return File(response, "application/pdf", Filename);
        }
    }
}
