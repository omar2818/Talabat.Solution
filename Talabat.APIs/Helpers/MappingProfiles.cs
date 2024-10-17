using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Helpers
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDTO>()
                .ForMember(d => d.Brand, O => O.MapFrom(P => P.Brand.Name))
                .ForMember(d => d.Category, O => O.MapFrom(P => P.Category.Name))
                //.ForMember(d => d.PictureUrl, O => O.MapFrom(P => $"{_configuration["APIBaseURL"]}/{P.PictureUrl}"));
                .ForMember(d => d.PictureUrl, O => O.MapFrom<ProductPictureURLRResolver>());

            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();

            CreateMap<Address, AddressDto>().ReverseMap();
            
            CreateMap<AddressDto, Address>();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryCost, O => O.MapFrom(S => S.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, O => O.MapFrom(S => S.ProductItemOrdered.ProductId))
                .ForMember(d => d.ProductName, O => O.MapFrom(S => S.ProductItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, O => O.MapFrom(S => S.ProductItemOrdered.PictureUrl))
                .ForMember(d => d.PictureUrl, O => O.MapFrom<OrderItemPictureUrlResolver>());
        }
    }
}
