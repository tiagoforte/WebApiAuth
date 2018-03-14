using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiToken.Model;

namespace WebApiToken.Interface
{
    public interface IProductRepository
    {
        IEnumerable<Product> Get();
        void Save(Product model);        

    }
}
