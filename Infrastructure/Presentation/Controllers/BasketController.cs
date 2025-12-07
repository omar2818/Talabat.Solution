using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DataTransferObjects.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class BasketController(IServiceManager _serviceManager) : ApiBaseController
    {
        // Get Basket
        [HttpGet] // GET: BaseUrl/api/Basket
        public async Task<ActionResult<BasketDto>> GetBasket(string key)
        {
            var Basket = await _serviceManager.BasketService.GetBasketAsync(key);
            return Ok(Basket);
        }

        // Create Or Update Basket
        [HttpPost] // POST: BaseUrl/api/Basket
        public async Task<ActionResult<BasketDto>> CreateOrUpdateBasket(BasketDto basket)
        {
            var Basket = await _serviceManager.BasketService.CreateOrUpdateBasketAsync(basket);
            return Ok(Basket);
        }

        // Delete Basket
        [HttpDelete("{key}")] // DELETE: BaseUrl/api/Basket/basket01
        public async Task<ActionResult<bool>> DeleteBasket(string key)
        {
            var result = await _serviceManager.BasketService.DeleteBasketAsync(key);
            return Ok(result);
        }
    }
}
