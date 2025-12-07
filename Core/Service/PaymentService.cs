using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.BasketModule;
using DomainLayer.Models.OrderModule;
using Microsoft.Extensions.Configuration;
using Service.Specifications;
using ServiceAbstraction;
using Shared.DataTransferObjects.BasketModule;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal class PaymentService(IConfiguration _configuration,
                                  IBasketRepository _basketRepository,
                                  IUnitOfWork _unitOfWork,
                                  IMapper _mapper) : IPaymentService
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration.GetSection("Stripe")["SecretKey"];

            var basket = await _basketRepository.GetBasketAsync(basketId);
            if(basket is null)
            {
                throw new BasketNotFoundException(basketId);
            }

            var productRepo = _unitOfWork.GetRepository<DomainLayer.Models.ProductModule.Product, int>();
            foreach(var item in basket.Items)
            {
                var product = await productRepo.GetbyIdAsync(item.Id);
                if(product is null)
                {
                    throw new ProductNotFoundException(item.Id);
                }

                item.Price = product.Price;
            }

            if(basket.DeliveryMethodId is null)
            {
                throw new ArgumentNullException();
            }
            var method = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetbyIdAsync(basket.DeliveryMethodId.Value);
            if(method is null)
            {
                throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
            }

            basket.ShippingPrice = method.Price;

            var amount = (long) (basket.Items.Sum(I => I.Price * I.Quantity) + method.Price) * 100L;

            var service = new PaymentIntentService();
            if (string.IsNullOrEmpty(basket.PaymentIntentId)) // Create
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = amount,
                    Currency = "AED",
                    PaymentMethodTypes = ["card"]
                };
                var paymentIntent = await service.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else // Update
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = amount
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            await _basketRepository.CreateOrUpdateBasketAsync(basket);
            return _mapper.Map<Basket, BasketDto>(basket);
        }

        public async Task UpdateOrderPaymentStatusAsync(string request, string stripeHeader)
        {
            var endPointSecret = _configuration.GetSection("Stripe")["EndPointSecret"];
            var stripeEvent = EventUtility.ConstructEvent(request, stripeHeader, endPointSecret);

            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            switch (stripeEvent.Type)
            {
                case EventTypes.PaymentIntentPaymentFailed:
                    await UpdatePaymentFailedAsync(paymentIntent.Id);
                    break;
                case EventTypes.PaymentIntentSucceeded:
                    await UpdatePaymentRecieviedAsync(paymentIntent.Id);
                    break;
                default:
                    Console.WriteLine($"Unhandled Stripe Event Type: {stripeEvent.Type}");
                    break;
            }
        }
        private async Task UpdatePaymentRecieviedAsync(string paymentIntentId)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>()
                                    .GetbyIdAsync(new OrderWithPaymentIntentSpecification(paymentIntentId));

            order.OrderStatus = OrderStatus.PaymentRecieved;
            _unitOfWork.GetRepository<Order, Guid>().Update(order);
            await _unitOfWork.SaveChangesAsync();
        }
        private async Task UpdatePaymentFailedAsync(string paymentIntentId)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>()
                                    .GetbyIdAsync(new OrderWithPaymentIntentSpecification(paymentIntentId));

            order.OrderStatus = OrderStatus.PaymentFailed;
            _unitOfWork.GetRepository<Order, Guid>().Update(order);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
