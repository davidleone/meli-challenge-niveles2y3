using AutoMapper;
using ChallengeMeLiServices.DataAccess.Models;
using ChallengeMeLiServices.Web.Models;

namespace ChallengeMeLiServices.Web.AutoMapper
{
    /// <summary>
    /// Mapping between DTOs and Entities.
    /// </summary>
    public static class AutoMapperWeb
    {
        /// <summary>
        /// Gets a new instance of configured Mapper.
        /// </summary>
        /// <returns>the IMapper instance</returns>
        public static IMapper GetMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Human, HumanV1Dto>()
                .ForMember(dto => dto.Version, opts => opts.Ignore())
                .ReverseMap();
            });
            return config.CreateMapper();
        }
    }
}