using AutoMapper;
using GSU.Museum.API.Data.Enums;
using GSU.Museum.API.Data.Models;
using GSU.Museum.API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GSU.Museum.API.Services
{
    public class StandsService : IStandsService
    {
        private readonly IStandsRepository _standsRepository;
        private readonly IExhibitsService _exhibitsService;

        public StandsService(IStandsRepository standsRepository, IExhibitsService exhibitsService)
        {
            _standsRepository = standsRepository;
            _exhibitsService = exhibitsService;
        }
        public async Task<List<StandDTO>> GetAllAsync(HttpRequest request, string hallId)
        {
            StringValues language = "";
            if (request != null)
            {
                if (!request.Headers.TryGetValue("Language", out language))
                {
                    language = "En";
                }
            }
            else
            {
                language = "En";
            }
            var stands = await _standsRepository.GetAllAsync(hallId);
            MapperConfiguration mapperConfiguration = null;
            List<StandDTO> standsDTO = new List<StandDTO>();
            if (stands != null)
            {
                switch (language)
                {
                    case "Ru":
                        mapperConfiguration = new MapperConfiguration(cfg => 
                        {
                            cfg.CreateMap<Stand, StandDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                                    source => source.TitleRu))
                            .ForMember(destination => destination.Description,
                                map => map.MapFrom(
                                    source => source.DescriptionRu))
                            .ForMember(destination => destination.Exhibits,
                                map => map.Ignore())
                            .ForMember(destination => destination.Photo,
                                map => map.Ignore());
                            cfg.AllowNullCollections = true;
                        });
                        break;
                    case "En":
                        mapperConfiguration = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<Stand, StandDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleEn))
                            .ForMember(destination => destination.Description,
                                map => map.MapFrom(
                                    source => source.DescriptionEn))
                            .ForMember(destination => destination.Exhibits,
                                map => map.Ignore())
                            .ForMember(destination => destination.Photo,
                                map => map.Ignore());
                            cfg.AllowNullCollections = true;
                        });
                        break;
                    case "Be":
                        mapperConfiguration = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<Stand, StandDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                                    source => source.TitleBe))
                            .ForMember(destination => destination.Description,
                                map => map.MapFrom(
                                    source => source.DescriptionBe))
                            .ForMember(destination => destination.Exhibits,
                                map => map.Ignore())
                            .ForMember(destination => destination.Photo,
                                map => map.Ignore());
                            cfg.AllowNullCollections = true;
                        });
                        break;
                    default:
                        mapperConfiguration = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<Stand, StandDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                                    source => source.TitleEn))
                            .ForMember(destination => destination.Description,
                                map => map.MapFrom(
                                    source => source.DescriptionEn))
                            .ForMember(destination => destination.Exhibits,
                                map => map.Ignore())
                            .ForMember(destination => destination.Photo,
                                map => map.Ignore());
                            cfg.AllowNullCollections = true;
                        });
                        break;
                }
                var mapper = new Mapper(mapperConfiguration);
                standsDTO = mapper.Map<List<StandDTO>>(stands);
            }
            return standsDTO;
        }

        public async Task<StandDTO> GetAsync(HttpRequest request, string hallId, string id)
        {
            StringValues language = "";
            if (request != null)
            {
                if (!request.Headers.TryGetValue("Language", out language))
                {
                    language = "En";
                }
            }
            else
            {
                language = "En";
            }
            var stand = await _standsRepository.GetAsync(hallId, id);
            MapperConfiguration mapperConfiguration = null;
            MapperConfiguration mapperConfigurationPhoto = null;
            StandDTO standDTO = null;
            if (stand != null)
            {
                switch (language)
                {
                    case "Ru":
                        mapperConfiguration = new MapperConfiguration(cfg => 
                        {
                            cfg.CreateMap<Stand, StandDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                                    source => source.TitleRu))
                            .ForMember(destination => destination.Description,
                                map => map.MapFrom(
                                    source => source.DescriptionRu))
                            .ForMember(destination => destination.Exhibits,
                                map => map.Ignore())
                            .ForMember(destination => destination.Photo,
                                map => map.Ignore());
                            cfg.AllowNullCollections = true;
                        });
                        mapperConfigurationPhoto = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<PhotoInfo, PhotoInfoDTO>()
                            .ForMember(destination => destination.Description,
                                map => map.MapFrom(
                                    source => source.DescriptionRu));
                        });
                        break;
                    case "En":
                        mapperConfiguration = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<Stand, StandDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                                    source => source.TitleEn))
                            .ForMember(destination => destination.Description,
                                map => map.MapFrom(
                                    source => source.DescriptionEn))
                            .ForMember(destination => destination.Exhibits,
                                map => map.Ignore())
                            .ForMember(destination => destination.Photo,
                                map => map.Ignore());
                            cfg.AllowNullCollections = true;
                        });
                        mapperConfigurationPhoto = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<PhotoInfo, PhotoInfoDTO>()
                            .ForMember(destination => destination.Description,
                                map => map.MapFrom(
                                    source => source.DescriptionEn));
                        });
                        break;
                    case "Be":
                        mapperConfiguration = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<Stand, StandDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                                    source => source.TitleBe))
                            .ForMember(destination => destination.Description,
                                map => map.MapFrom(
                                    source => source.DescriptionBe))
                            .ForMember(destination => destination.Exhibits,
                                map => map.Ignore())
                            .ForMember(destination => destination.Photo,
                                map => map.Ignore());
                            cfg.AllowNullCollections = true;
                        });
                        mapperConfigurationPhoto = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<PhotoInfo, PhotoInfoDTO>()
                            .ForMember(destination => destination.Description,
                                map => map.MapFrom(
                                    source => source.DescriptionBe));
                        });
                        break;
                    default:
                        mapperConfiguration = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<Stand, StandDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                                    source => source.TitleEn))
                            .ForMember(destination => destination.Description,
                                map => map.MapFrom(
                                    source => source.DescriptionEn))
                            .ForMember(destination => destination.Exhibits,
                                map => map.Ignore())
                            .ForMember(destination => destination.Photo,
                                map => map.Ignore());
                            cfg.AllowNullCollections = true;
                        });
                        mapperConfigurationPhoto = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<PhotoInfo, PhotoInfoDTO>()
                            .ForMember(destination => destination.Description,
                                map => map.MapFrom(
                                source => source.DescriptionEn));
                        });
                        break;
                }
                var mapper = new Mapper(mapperConfiguration);
                standDTO = mapper.Map<StandDTO>(stand);

                mapper = new Mapper(mapperConfigurationPhoto);
                var photoInfoDTO = mapper.Map<PhotoInfoDTO>(stand.Photo);
                standDTO.Photo = photoInfoDTO;

                var exhibits = await _exhibitsService.GetAllAsync(request, hallId, id);
                standDTO.Exhibits = exhibits;
            }
            return standDTO;
        }
    }
}
