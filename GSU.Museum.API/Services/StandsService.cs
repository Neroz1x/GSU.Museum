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
    public class StandsService : IStandsService
    {
        private readonly IStandsRepository _standsRepository;
        private readonly IExhibitsService _exhibitsService;

        public StandsService(IStandsRepository standsRepository, IExhibitsService exhibitsService)
        {
            _standsRepository = standsRepository;
            _exhibitsService = exhibitsService;
        }
        public async Task<List<StandDTO>> GetAllAsync(StringValues language, string hallId)
        {
            var stands = await _standsRepository.GetAllAsync(hallId);
            MapperConfiguration mapperConfiguration = null;
            List<StandDTO> standsDTO = new List<StandDTO>();
            if (stands != null)
            {
                switch (language)
                {
                    case "Ru":
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Stand, StandDTO>()
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleRu))
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleRu))
                        .ForMember(destination => destination.Text,
                               map => map.Ignore())
                        .ForMember(destination => destination.Exhibits,
                            map => map.Ignore())
                        );
                        break;
                    case "En":
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Stand, StandDTO>()
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleEn))
                        .ForMember(destination => destination.Text,
                                map => map.Ignore())
                        .ForMember(destination => destination.Exhibits,
                            map => map.Ignore())
                        );
                        break;
                    case "Be":
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Stand, StandDTO>()
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleBe))
                        .ForMember(destination => destination.Text,
                                map => map.Ignore())
                        .ForMember(destination => destination.Exhibits,
                            map => map.Ignore())
                        );
                        break;
                    default:
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Stand, StandDTO>()
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleEn))
                        .ForMember(destination => destination.Text,
                                map => map.Ignore())
                        .ForMember(destination => destination.Exhibits,
                            map => map.Ignore())
                        );
                        break;
                }
                var mapper = new Mapper(mapperConfiguration);
                standsDTO = mapper.Map<List<StandDTO>>(stands);
                if (standsDTO.First(el => el.Text?.Count == 0 || el.Text == null) != null)
                {
                    throw new Error(Errors.Not_found, $"There is no text in {language} language");
                }
                if (standsDTO.First(el => string.IsNullOrEmpty(el.Title)) != null)
                {
                    throw new Error(Errors.Not_found, $"There is no title in {language} language");
                }
            }
            return standsDTO;
        }

        public async Task<StandDTO> GetAsync(StringValues language, string hallId, string id)
        {
            var stand = await _standsRepository.GetAsync(hallId, id);
            MapperConfiguration mapperConfiguration = null;
            StandDTO standDTO = new StandDTO();
            if (stand != null)
            {
                switch (language)
                {
                    case "Ru":
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Stand, StandDTO>()
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleRu))
                        .ForMember(destination => destination.Exhibits,
                            map => map.Ignore())
                        );
                        break;
                    case "En":
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Stand, StandDTO>()
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleEn))
                        .ForMember(destination => destination.Exhibits,
                            map => map.Ignore())
                        );
                        break;
                    case "Be":
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Stand, StandDTO>()
                         .ForMember(destination => destination.Title,
                                 map => map.MapFrom(
                             source => source.TitleBe))
                         .ForMember(destination => destination.Exhibits,
                             map => map.Ignore())
                         );
                        break;
                    default:
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Stand, StandDTO>()
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleEn))
                        .ForMember(destination => destination.Exhibits,
                            map => map.Ignore())
                        );
                        break;
                }
                var mapper = new Mapper(mapperConfiguration);
                standDTO = mapper.Map<StandDTO>(stand);

                var exhibits = await _exhibitsService.GetAllAsync(language, hallId, id);
                standDTO.Exhibits = exhibits;
                if (standDTO?.Text.Count == 0 || standDTO.Text == null)
                {
                    throw new Error(Errors.Not_found, $"There is no text in {language} language");
                }
                if (string.IsNullOrEmpty(standDTO.Title))
                {
                    throw new Error(Errors.Not_found, $"There is no title in {language} language");
                }
            }
            return standDTO;
        }
    }
}
