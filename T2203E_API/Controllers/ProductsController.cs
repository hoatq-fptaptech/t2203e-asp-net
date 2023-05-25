using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using T2203E_API.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace T2203E_API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        // Scaffold-DbContext "connection string" Microsoft.EntityFrameworkCore.SqlServer - OutputDir Entities - Force
        // dotnet ef dbcontext scaffold "Data Source=localhost,1433; Database=T2203E_API;User Id=sa;Password=sa123456;TrustServerCertificate=true" Microsoft.EntityFrameworkCore.SqlServer --output-dir=Entities --force

        private readonly T2203eApiContext _context;
        public ProductsController(T2203eApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var products = _context.Products.Include(p=>p.Category)
                .ToArray();
            //foreach(var p in products)
            //{
            //    Console.WriteLine(p.Category.Name);
            //}
            return Ok(products);
        }

        [HttpGet]
        [Route("get-by-id")]
        public IActionResult Get(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return Created($"/get-by-id?id={product.Id}", product);
        }

        [HttpPut]
        public IActionResult Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var productDelete = _context.Products.Find(id);
            if (productDelete == null)
                return NotFound();
            _context.Products.Remove(productDelete);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpGet]
        [Route("search")]
        public IActionResult Search(string? q,int? limit,int? page,[FromHeader] int userId)
        {
            limit = limit != null ? limit : 10;
            page = page != null ? page : 1;
            int offset = (int)((page - 1) * limit);
            var products = _context.Products.Where(p => p.Name.Contains(q))
                .Skip(offset).Take((int)limit).ToArray();
            return Ok(products);
        }
    }
}

