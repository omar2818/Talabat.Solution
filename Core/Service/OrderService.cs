using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.OrderModule;
using DomainLayer.Models.ProductModule;
using Service.Specifications;
using ServiceAbstraction;
using Shared.DataTransferObjects.IdentityModule;
using Shared.DataTransferObjects.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal class OrderService(IMapper _mapper, IBasketRepository _basketRepository, IUnitOfWork _unitOfWork) : IOrderService
    {
        public async Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderDto, string email)
        {
            // Map AddressDto to Order Address
            var orderAddress = _mapper.Map<AddressDto, OrderAddress>(orderDto.Address);

            var basket = await _basketRepository.GetBasketAsync(orderDto.BasketId);
            if (basket is null)
                throw new BasketNotFoundException(orderDto.BasketId);

            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var existingOrderSpecs = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId!);
            var existingOrder = await orderRepo.GetbyIdAsync(existingOrderSpecs);
            if(existingOrder is not null)
            {
                orderRepo.Delete(existingOrder);
            }

            // Create OrderItem List
            List<OrderItem> orderItems = [];
            var productRepo = _unitOfWork.GetRepository<Product, int>();
            foreach(var item in basket.Items)
            {
                var product = await productRepo.GetbyIdAsync(item.Id);
                if(product is null)
                    throw new ProductNotFoundException(item.Id);
                var orderItem = new OrderItem()
                {
                    Product = new ProductItemOrdered()
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        PictureUrl = product.PictureUrl,
                    },
                    Price = product.Price,
                    Quantity = item.Quantity
                };
                orderItems.Add(orderItem);
            }

            // Get Delivery Method
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetbyIdAsync(orderDto.DeliveryMethodId);
            if(deliveryMethod is null)
                throw new DeliveryMethodNotFoundException(orderDto.DeliveryMethodId);

            var subTotal = orderItems.Sum(OI => OI.Price * OI.Quantity);

            var order = new Order(email, orderAddress, deliveryMethod, orderItems, subTotal, basket.PaymentIntentId!);
            await _unitOfWork.GetRepository<Order, Guid>().AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<Order, OrderToReturnDto>(order);
        }

        public async Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMethod>, IEnumerable<DeliveryMethodDto>>(deliveryMethods);
        }
        public async Task<IEnumerable<OrderToReturnDto>> GetAllOrdersAsync(string email)
        {
            var spec = new OrderSpecifications(email);
            var orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(spec);
            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderToReturnDto>>(orders);
        }

        public async Task<OrderToReturnDto> GetOrderByIdAsync(Guid id)
        {
            var spec = new OrderSpecifications(id);
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetbyIdAsync(spec);
            return _mapper.Map<Order, OrderToReturnDto>(order);
        }
    }
}
