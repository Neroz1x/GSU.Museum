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
            var halls = await _hallsRepository.GetAllAsync();
            MapperConfiguration mapperConfiguration = null;
            List<HallDTO> hallsDTO = new List<HallDTO>();
            if (halls != null)
            {
                switch (language)
                {
                    case "Ru":
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
                    case "En":
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
                    case "Be":
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
                hallsDTO = mapper.Map<List<HallDTO>>(halls);
                if (hallsDTO.FirstOrDefault(el => string.IsNullOrEmpty(el.Title)) != null)
                {
                    throw new Error(Errors.Not_found, $"There is no title in {language} language");
                }
            }
            return hallsDTO;
        }

        public async Task<HallDTO> GetAsync(HttpRequest request, string id)
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
            var hall = await _hallsRepository.GetAsync(id);
            MapperConfiguration mapperConfiguration = null;
            MapperConfiguration mapperConfigurationPhoto = null;
            HallDTO hallDTO = null;
            if (hall != null)
            {
                switch (language)
                {
                    case "Ru":
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
                    case "En":
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
                    case "Be":
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
                var mapper = new Mapper(mapperConfiguration);
                hallDTO = mapper.Map<HallDTO>(hall);
                
                mapper = new Mapper(mapperConfigurationPhoto);
                var photoInfoDTO = mapper.Map<PhotoInfoDTO>(hall.Photo);
                hallDTO.Photo = photoInfoDTO;
                
                var stands = await _standsService.GetAllAsync(request, id);
                hallDTO.Stands = stands;
                
                if (string.IsNullOrEmpty(hallDTO.Title))
                {
                    throw new Error(Errors.Not_found, $"There is no title in {language} language");
                }
            }
            return hallDTO;
        }
    }
}
