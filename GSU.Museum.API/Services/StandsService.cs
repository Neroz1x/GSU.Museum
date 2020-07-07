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
                if (standsDTO.FirstOrDefault(el => string.IsNullOrEmpty(el.Title)) != null)
                {
                    throw new Error(Errors.Not_found, $"There is no title in {language} language");
                }
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
            StandDTO standDTO = null;
            if (stand != null)
            {
                switch (language)
                {
                    case "Ru":
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Stand, StandDTO>()
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleRu))
                        .ForMember(destination => destination.Text,
                                map => map.MapFrom(
                            source => source.TextRu))
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
                                map => map.MapFrom(
                            source => source.TextEn))
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
                                map => map.MapFrom(
                            source => source.TextBe))
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
                                map => map.MapFrom(
                            source => source.TextEn))
                        .ForMember(destination => destination.Exhibits,
                            map => map.Ignore())
                        );
                        break;
                }
                var mapper = new Mapper(mapperConfiguration);
                standDTO = mapper.Map<StandDTO>(stand);

                var exhibits = await _exhibitsService.GetAllAsync(request, hallId, id);
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
