using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }

        public void Update(Product product)
        {
            //_db.Products.Update(product);
            
            var productDb = _db.Products.FirstOrDefault(_=> _.Id == product.Id);

            if (productDb != null)
            {
                productDb.Title = product.Title;
                productDb.Description = product.Description;
                productDb.ISBN = product.ISBN;
                productDb.ListPrice = product.ListPrice;                
                productDb.Price = product.Price;
                product.Price50 = product.Price50;
                product.Price100 = product.Price100;
                product.CategoryId = product.CategoryId;
                product.Author = product.Author;

                if (product.ImageUrl != null)
                {
                    productDb.ImageUrl = product.ImageUrl;
                }
            }
        }
    }
}
