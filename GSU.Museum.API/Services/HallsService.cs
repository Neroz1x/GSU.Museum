using AutoMapper;
using GSU.Museum.API.Data.Enums;
using GSU.Museum.API.Data.Models;
using GSU.Museum.API.Interfaces;
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

        public async Task<List<HallDTO>> GetAllAsync(StringValues language)
        {
            var halls = await _hallsRepository.GetAllAsync();
            MapperConfiguration mapperConfiguration = null;
            List<HallDTO> hallsDTO = new List<HallDTO>();
            if (halls!= null)
            {
                switch (language)
                {
                    case "Ru":
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Hall, HallDTO>()
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleRu))
                        .ForMember(destination => destination.Stands,
                            map => map.Ignore())
                        );
                        break;
                    case "En":
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Hall, HallDTO>()
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleEn))
                        .ForMember(destination => destination.Stands,
                            map => map.Ignore())
                        );
                        break;
                    case "Be":
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Hall, HallDTO>()
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleBe))
                        .ForMember(destination => destination.Stands,
                            map => map.Ignore())
                        );
                        break;
                    default:
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Hall, HallDTO>()
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleEn))
                        .ForMember(destination => destination.Stands,
                            map => map.Ignore())
                        );
                        break;
                }
                var mapper = new Mapper(mapperConfiguration);
                hallsDTO = mapper.Map<List<HallDTO>>(halls);
                if (hallsDTO.First(el => string.IsNullOrEmpty(el.Title)) != null)
                {
                    throw new Error(Errors.Not_found, $"There is no title in {language} language");
                }
            }
            return hallsDTO;
        }

        public async Task<HallDTO> GetAsync(StringValues language, string id)
        {
            var hall = await _hallsRepository.GetAsync(id);
            MapperConfiguration mapperConfiguration = null;
            HallDTO hallDTO = new HallDTO();
            if (hall != null)
            {
                switch (language)
                {
                    case "Ru":
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Hall, HallDTO>()
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleRu))
                        .ForMember(destination => destination.Stands,
                            map => map.Ignore())
                        );
                        break;
                    case "En":
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Hall, HallDTO>()
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleEn))
                        .ForMember(destination => destination.Stands,
                            map => map.Ignore())
                        );
                        break;
                    case "Be":
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Hall, HallDTO>()
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleBe))
                        .ForMember(destination => destination.Stands,
                            map => map.Ignore())
                        );
                        break;
                    default:
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Hall, HallDTO>()
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleEn))
                        .ForMember(destination => destination.Stands,
                            map => map.Ignore())
                        );
                        break;
                }
                var mapper = new Mapper(mapperConfiguration);
                hallDTO = mapper.Map<HallDTO>(hall);

                var stands = await _standsService.GetAllAsync(language, id);
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
