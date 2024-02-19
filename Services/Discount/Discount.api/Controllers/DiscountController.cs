using Discount.api.Entities;
using Discount.api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Discount.api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        #region constructor
        private readonly IDiscountRepository _discountRepository;
        public DiscountController( IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }
        #endregion
        #region get discount
        [HttpGet("{productName}", Name ="GetDiscount")]
        [ProducesResponseType(typeof(Coupon),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>>GetDiscount(string productName)
        {
            var coupon =await _discountRepository.GetDiscount(productName);
            return Ok(coupon);
        }
        #endregion
        #region create discount
        [HttpPost()]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon)
        {
            var result = await _discountRepository.CreateDiscount(coupon);
            return CreatedAtRoute("GetDiscount",new {productName = coupon.ProductName},coupon);
        }
        #endregion
        #region update discount
        [HttpPut()]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> UpdateDiscount([FromBody] Coupon coupon)
        {
            return Ok(await _discountRepository.UpdateDiscount(coupon));
        }
        #endregion
        #region delete discount
        [HttpDelete("{procustName}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> DeleteDiscount(string procustName)
        {
            return Ok(await _discountRepository.DeleteDiscount(procustName));
        }
        #endregion
    }
}
