using AdminDashboardWorkshop.Models;
using AutoMapper;
using Talabat.Core.Entities;

namespace AdminDashboardWorkshop.Helpers
{
    public class MapsProfile: Profile
    {
        public MapsProfile()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();
        }
    }
}
