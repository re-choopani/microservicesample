using AutoMapper;
using Basket.Api.Entities;
using Basket.Api.GrpcServices;
using Basket.Api.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.NetworkInformation;

namespace Basket.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        #region constractor
        private readonly IBasketRepository _basketRepository;
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly ILogger<BasketController> _logger;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountGrpcService, ILogger<BasketController> logger, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _basketRepository = basketRepository;
            _discountGrpcService = discountGrpcService;
            _logger = logger;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }
        #endregion
        #region GetBasket
        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _basketRepository.GetUserBasket(userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }
        #endregion
        #region update basket
        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            foreach (var item in basket.Items)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                item.Price += coupon.Amount;
                //try
                //{
                //    var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                //    item.Price += coupon.Amount;
                //}
                //catch (Exception e)
                //{
                //    _logger.LogError(e.Message + e.InnerException?.Message);
                //}

            }
            return Ok(await _basketRepository.UpdateBasket(basket));
        }
        #endregion
        #region remove basket
        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _basketRepository.DeleteBasket(userName);
            return Ok();
        }
        #endregion

        #region checkout
        [HttpPost("[action]")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody]BasketCheckout basketCheckout)
        {

            var basket = await _basketRepository.GetUserBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;

            await _publishEndpoint.Publish(eventMessage);

            await _basketRepository.DeleteBasket(basketCheckout.UserName);
            return Accepted();
        }
        #endregion
    }
}
