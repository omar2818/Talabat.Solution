using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;

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

            CreateMap<Address, AddressDto>();
		}
    }
}
