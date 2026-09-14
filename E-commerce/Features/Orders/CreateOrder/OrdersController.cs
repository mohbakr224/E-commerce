using E_commerce.Features.Orders.CreateOrder;
using E_commerce.Features.Orders.GetOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_commerce.Features.Orders.CreateOrder
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var dto = await _mediator.Send(new GetOrderQuery { Id = id });
            if (dto == null) return NotFound();
            return Ok(dto);
        }
    }
}
