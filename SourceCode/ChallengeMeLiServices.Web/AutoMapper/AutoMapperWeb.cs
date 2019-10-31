using AutoMapper;
using ChallengeMeLiServices.Services.Models;
using ChallengeMeLiServices.Web.Models;

namespace ChallengeMeLiServices.Web.AutoMapper
{
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
                cfg.CreateMap<Human, HumanV1Dto>().ReverseMap();
            });
            return config.CreateMapper();
        }
    }
}