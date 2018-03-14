using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebApiToken.Interface;

namespace WebApiToken.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository _produtRepository;

        public ProductController(IProductRepository produtRepository)
        {
            _produtRepository = produtRepository;

        }

        [HttpGet]
        [Route("v1/product")]
        [Authorize]
        public async Task<IActionResult> Get()
        {


            return Ok(_produtRepository.Get());
        }
    }
}
