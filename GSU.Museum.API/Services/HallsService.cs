using AutoMapper;
using GSU.Museum.API.Interfaces;
using GSU.Museum.CommonClassLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSU.Museum.API.Services
{
    public class HallsService : IHallsService
    {
        private readonly IHallsRepository _hallsRepository;
        private readonly IStandsService _standsService;
        public HallsService(IHallsRepository hallsRepository, IStandsService standsService)
        {
            _hallsRepository = hallsRepository;
            _standsService = standsService;
        }

        public async Task<List<HallDTO>> GetAllAsync(HttpRequest request)
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
            var halls = await _hallsRepository.GetAllAsync();
            MapperConfiguration mapperConfiguration = null;
            MapperConfiguration mapperConfigurationPhoto = null;
            List<HallDTO> hallsDTO = new List<HallDTO>();
            if (halls != null)
            {
                // Create mapping depending on language
                switch (language)
                {
                    case "ru":
                        mapperConfiguration = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<Hall, HallDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                                    source => source.TitleRu))
                            .ForMember(destination => destination.Stands,
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
                            cfg.CreateMap<Hall, HallDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                                source => source.TitleEn))
                            .ForMember(destination => destination.Stands,
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
                            cfg.CreateMap<Hall, HallDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                                    source => source.TitleBe))
                            .ForMember(destination => destination.Stands,
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
                            cfg.CreateMap<Hall, HallDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                                    source => source.TitleEn))
                            .ForMember(destination => destination.Stands,
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
            }
            var mapper = new Mapper(mapperConfiguration);
            hallsDTO = mapper.Map<List<HallDTO>>(halls);

            mapper = new Mapper(mapperConfigurationPhoto);
            for (int i = 0; i < halls.Count; i++)
            {
                var photoInfoDTO = mapper.Map<PhotoInfoDTO>(halls[i].Photo);
                hallsDTO[i].Photo = photoInfoDTO;
            }
            
            return hallsDTO;
        }

        public async Task<HallDTO> GetAsync(HttpRequest request, string id)
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
            var hall = await _hallsRepository.GetAsync(id);
            MapperConfiguration mapperConfiguration = null;
            HallDTO hallDTO = null;
            if (hall != null)
            {
                // Create mapping depending on language
                switch (language)
                {
                    case "ru":
                        mapperConfiguration = new MapperConfiguration(cfg => 
                        {
                            cfg.CreateMap<Hall, HallDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                                    source => source.TitleRu))
                            .ForMember(destination => destination.Stands,
                                map => map.Ignore())
                            .ForMember(destination => destination.Photo,
                                map => map.Ignore());
                            cfg.AllowNullCollections = true;
                        });
                        break;
                    case "en":
                        mapperConfiguration = new MapperConfiguration(cfg => 
                        {
                            cfg.CreateMap<Hall, HallDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                                    source => source.TitleEn))
                            .ForMember(destination => destination.Stands,
                                map => map.Ignore())
                            .ForMember(destination => destination.Photo,
                                map => map.Ignore());
                            cfg.AllowNullCollections = true;
                        });
                        break;
                    case "be":
                        mapperConfiguration = new MapperConfiguration(cfg => 
                        {
                            cfg.CreateMap<Hall, HallDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                                    source => source.TitleBe))
                            .ForMember(destination => destination.Stands,
                                map => map.Ignore())
                            .ForMember(destination => destination.Photo,
                                map => map.Ignore());
                            cfg.AllowNullCollections = true;
                        });
                        break;
                    default:
                        mapperConfiguration = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<Hall, HallDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                                source => source.TitleEn))
                            .ForMember(destination => destination.Stands,
                                map => map.Ignore())
                            .ForMember(destination => destination.Photo,
                                map => map.Ignore());
                            cfg.AllowNullCollections = true;
                        });
                        break;
                }
                var mapper = new Mapper(mapperConfiguration);
                hallDTO = mapper.Map<HallDTO>(hall);
                
                var stands = await _standsService.GetAllAsync(request, id);
                hallDTO.Stands = stands;
            }
            return hallDTO;
        }
    }
}
