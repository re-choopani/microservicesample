using AutoMapper;
using Discount.Grpc.Entities;
using DiscountGrpc.Protos;

namespace DiscountGrpc.Mapper
{
    public class DiscountProfile:Profile
    {
        public DiscountProfile()
        {
            CreateMap<Coupon, CouponModel>();
            CreateMap<CouponModel, Coupon>();
        }
    }
}
