using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetordersList;
using System.Net;

namespace Ordering.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        #region constructor
        private readonly IMediator _mediator;
        public OrderController(IMediator mediator = null)
        {
            _mediator = mediator;
        }
        #endregion
        #region get all orders
        [HttpGet("{userName}",Name = "GetOrders")]
        [ProducesResponseType(typeof(IEnumerable<OrdersVm>),(int) HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrdersVm>>>GetOrderByUserName(string userName)
        {
            var query = new GetOrdersListQuery(userName);

            var orders = await _mediator.Send(query);
            return Ok(orders);
        }
        #endregion
        #region checkout order
        [HttpPost(Name = "CheckoutOrder")]
        [ProducesResponseType(typeof(int),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderCommand command)
        {
            var resault = await _mediator.Send(command);
            return Ok(resault);
        }
        #endregion
        #region update order
        [HttpPut("UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateOrder([FromBody]UpdateOrderCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
        #endregion
        #region delete order
        [HttpDelete("{id}",Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            await _mediator.Send(id);
            return NoContent();
        }
        #endregion
    }
}
