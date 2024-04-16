using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;

namespace Talabat.APIs.Helpers
{
	public class MappingProfiles : Profile
	{
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDTO>()
                .ForMember(d => d.Brand, O => O.MapFrom(P => P.Brand.Name))
                .ForMember(d => d.Category, O => O.MapFrom(P => P.Category.Name));
        }
    }
}
