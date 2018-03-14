using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiToken.Data;
using WebApiToken.Interface;
using WebApiToken.Model;

namespace WebApiToken.Repository
{
    public class ProductRepository : IProductRepository
    {
        private DataContext _context;
        public ProductRepository(DataContext context)
        {
            _context = context;


        }

        public IEnumerable<Product> Get()
        {
            return _context.Product.ToList();
        }

        public void Save(Product model)
        {
            throw new NotImplementedException();
        }
      

    }
}
