using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiDemo.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductContext context;

        public ProductsController(ProductContext _context)
        {
            context = _context;
            if (context.Products.Count() == 0)
            {
                context.Products.Add(new ProductModel { Id = 1, Name = "Soap", Price = 12.5M });
                context.SaveChanges();
            }
        }

        [HttpGet]
        public ActionResult<List<ProductModel>> GetAll()
        {
            return context.Products.ToList();
        }

        [HttpGet("{id}", Name = "GetProduct")]
        public ActionResult<ProductModel> GetById(int id)
        {
            var product = context.Products.Find(id);
            return product == null ? (ActionResult<ProductModel>)NotFound() : (ActionResult<ProductModel>)product;
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult<ProductModel> Create(ProductModel model)
        {
            context.Products.Add(model);
            context.SaveChanges();
            return CreatedAtRoute("GetProduct", new { id = model.Id }, model);
        }
    }
}