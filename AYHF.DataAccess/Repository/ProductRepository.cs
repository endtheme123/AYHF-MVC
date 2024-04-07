using AYHF.DataAccess.Data;
using AYHF.DataAccess.Repository.IRepository;
using AYHF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYHF.DataAccess.Repository
{
    public class ProductRepository:Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }

        public void Update(Product product)
        {
            //_db.Update(product);
            var obj = _db.Products.FirstOrDefault(p => p.Id == product.Id);
            if(obj != null)
            {
                obj.Name = product.Name;
                obj.Description = product.Description;
                obj.Price = product.Price;
                obj.Stock = product.Stock;
                obj.Ordered = product.Ordered;
                if(product.ImageUrl != null)
                {
                    obj.ImageUrl = product.ImageUrl;
                }
            }
        }
    }
}
