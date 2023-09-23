using AutoMapper;
using ServiceRepairComputer.Models;
using Warehouse_API.Models;
using Warehouse_API.Models.Dto;

namespace Warehouse_API
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingCongfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Division, DivisionDto>();
                config.CreateMap<DivisionDto, Division>();
                config.CreateMap<IncomingStock, IncomingStockDto>();
                config.CreateMap<IncomingStockDto, IncomingStock>();
                config.CreateMap<OutgoingStock, OutgoingStockDto>();
                config.CreateMap<OutgoingStockDto, OutgoingStock>();
                config.CreateMap<Position, PositionDto>();
                config.CreateMap<PositionDto, Position>();
                config.CreateMap<Product, ProductDto>();
                config.CreateMap<ProductDto, Product>();
                config.CreateMap<Users, UsersDto>();
                config.CreateMap<UsersDto, Users>();
                config.CreateMap<ProductType, ProductTypeDto>();
                config.CreateMap<ProductTypeDto, ProductType>();
            });

            return mappingCongfig;
        }
    }
}
