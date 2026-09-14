using E_commerce.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace E_commerce.Features.Orders.Dashboard
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDB _db;

        public DashboardController(ApplicationDB db)
        {
            _db = db;
        }

        [HttpGet("orders")]
        public IActionResult GetOrders()
        {
            var data = _db.DashboardOrders.ToList();
            return Ok(data);
        }
    }
}
