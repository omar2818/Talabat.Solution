using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using DomainLayer.Models.ProductModule;
using Shared.DataTransferObjects.ProductModule;

namespace Service.MappingProfiles
{
    internal class PictureUrlResolver(IConfiguration configuration) : IValueResolver<Product, ProductDto, string>
    {
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.PictureUrl))
            {
                return string.Empty;
            }
            return $"{configuration.GetSection("Urls")["BaseUrl"]}{source.PictureUrl}";
        }
    }
}
