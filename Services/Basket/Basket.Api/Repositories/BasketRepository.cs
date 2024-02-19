using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.Api.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCasche;
        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCasche = redisCache;
        }

        public async Task<ShoppingCart> GetUserBasket(string userName)
        {
            var basket = await _redisCasche.GetStringAsync(userName);
            if (string.IsNullOrEmpty(basket))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await _redisCasche.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
            return await GetUserBasket(basket.UserName);
        }
        public async Task DeleteBasket(string userName)
        {
            await _redisCasche.RemoveAsync(userName);
        }
    }
}
