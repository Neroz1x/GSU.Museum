using AutoMapper;
using GSU.Museum.API.Interfaces;
using GSU.Museum.CommonClassLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
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
            // Get language from header
            StringValues language = "";
            if (request != null)
            {
                if (!request.Headers.TryGetValue("Accept-Language", out language))
                {
                    language = "en";
                }
            }
            else
            {
                language = "en";
            }
            var stands = await _standsRepository.GetAllAsync(hallId);
            MapperConfiguration mapperConfiguration = null;
            MapperConfiguration mapperConfigurationPhoto = null;
            List<StandDTO> standsDTO = new List<StandDTO>();
            if (stands != null)
            {
                // Create mapping depending on language
                switch (language)
                {
                    case "ru":
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
                    case "en":
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
                    case "be":
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
                standsDTO = mapper.Map<List<StandDTO>>(stands);
                mapper = new Mapper(mapperConfigurationPhoto);
                for(int i = 0; i < stands.Count; i++)
                {
                    var photoInfoDTO = mapper.Map<PhotoInfoDTO>(stands[i].Photo);
                    standsDTO[i].Photo = photoInfoDTO;
                }
            }
            return standsDTO;
        }

        public async Task<StandDTO> GetAsync(HttpRequest request, string hallId, string id)
        {
            // Get language from header
            StringValues language = "";
            if (request != null)
            {
                if (!request.Headers.TryGetValue("Accept-Language", out language))
                {
                    language = "en";
                }
            }
            else
            {
                language = "en";
            }
            var stand = await _standsRepository.GetAsync(hallId, id);
            MapperConfiguration mapperConfiguration = null;
            StandDTO standDTO = null;
            if (stand != null)
            {
                // Create mapping depending on language
                switch (language)
                {
                    case "ru":
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
                    case "en":
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
                    case "be":
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
                standDTO = mapper.Map<StandDTO>(stand);

                var exhibits = await _exhibitsService.GetAllAsync(request, hallId, id);
                standDTO.Exhibits = exhibits;
            }
            return standDTO;
        }
    }
}
