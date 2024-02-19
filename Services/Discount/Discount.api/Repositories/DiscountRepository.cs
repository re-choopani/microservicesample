using Discount.api.Entities;
using Npgsql;
using Dapper;

namespace Discount.api.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        #region constructor
        private readonly IConfiguration _configuration;
 
        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion
        #region create
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync("insert into Coupon(ProductName,Description,Amount)values(@ProductName,@Description,@Amount)", new { coupon.ProductName, coupon.Description, coupon.Amount });
            if (affected == 0)
            {
                return false;
            }
            return true;
        }
        #endregion
        #region delete
        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync("delete from Coupon  where ProductName =@ProductName", new { ProductName = productName });
            if (affected == 0)
            {
                return false;
            }
            return true;
        }
        #endregion
        #region get discount
        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("select * from coupon where ProductName = @ProductName", new { ProductName = productName });
            if (coupon == null) { return new Coupon { ProductName = "No Discount" }; }
            return coupon;
        }
        #endregion
        #region update discount
        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected = await connection.ExecuteAsync("update Coupon set ProductName=@ProductName,Description=@Description,Amount=@Amount where ID =@ID", new { coupon.ProductName,coupon.Description,coupon.Amount,coupon.ID});
            if (affected == 0)
            {
                return false;
            }
            return true;
        }
        #endregion

    }
}
