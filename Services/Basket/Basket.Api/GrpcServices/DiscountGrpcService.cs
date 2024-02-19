using DiscountGrpc.Protos;

namespace Basket.Api.GrpcServices
{
    public class DiscountGrpcService
    {
        #region constructor
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
        {
            _discountProtoServiceClient = discountProtoServiceClient;
        }
        #endregion
        #region get discount
        public async Task<CouponModel>GetDiscount(string productName)
        {
            var discountRequest = new GetDiscountRequest { ProductName = productName };
            return await _discountProtoServiceClient.GetDiscountAsync(discountRequest);
        }
        #endregion
    }
}
