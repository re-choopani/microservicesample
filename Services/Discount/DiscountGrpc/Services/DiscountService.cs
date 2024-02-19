using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Repositories;
using DiscountGrpc.Protos;
using Grpc.Core;

namespace DiscountGrpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        #region constractor
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;
        public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountService> logger, IMapper mapper)
        {
            _discountRepository = discountRepository;
            _logger = logger;
            _mapper = mapper;
        }
        #endregion
        #region get discount
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _discountRepository.GetDiscount(request.ProductName);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "discount not found"));
            }
            _logger.LogInformation("discount is retrived");
            return _mapper.Map<CouponModel>(coupon);
        }
        #endregion
        #region create discount
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            await _discountRepository.CreateDiscount(coupon);
            _logger.LogInformation("add coupon");
            return _mapper.Map<CouponModel>(coupon);
        }
        #endregion
        #region update disount
        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            await _discountRepository.UpdateDiscount(coupon);
            _logger.LogInformation("update coupon");
            return _mapper.Map<CouponModel>(coupon);
        }
        #endregion
        #region delete discount
        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await _discountRepository.DeleteDiscount(request.ProductName);
            return new DeleteDiscountResponse
            {
                Success = deleted,
            };
        }
        #endregion
    }
}
