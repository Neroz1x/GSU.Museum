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
    public class ExhibitsService : IExhibitsService
    {
        private readonly IExhibitsRepository _exhibitsRepository;

        public ExhibitsService(IExhibitsRepository exhibitsRepository)
        {
            _exhibitsRepository = exhibitsRepository;
        }

        public async Task<List<ExhibitDTO>> GetAllAsync(HttpRequest request, string hallId, string standId)
        {
            StringValues language = "";
            if(request != null)
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
            var exhibits = await _exhibitsRepository.GetAllAsync(hallId, standId);
            MapperConfiguration mapperConfiguration = null;
            List<ExhibitDTO> exhibitsDTO = new List<ExhibitDTO>();
            if (exhibits != null)
            {
                switch (language)
                {
                    case "Ru":
                        mapperConfiguration = new MapperConfiguration(cfg => 
                        {
                            cfg.CreateMap<Exhibit, ExhibitDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleRu))
                            .ForMember(destination => destination.Text,
                               map => map.Ignore())
                            .ForMember(destination => destination.Photos,
                            map => map.Ignore());
                            cfg.AllowNullCollections = true;
                        });
                        break;
                    case "En":
                        mapperConfiguration = new MapperConfiguration(cfg => 
                        {
                            cfg.CreateMap<Exhibit, ExhibitDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleEn))
                            .ForMember(destination => destination.Text,
                               map => map.Ignore())
                            .ForMember(destination => destination.Photos,
                            map => map.Ignore());
                            cfg.AllowNullCollections = true;
                        });
                        break;
                    case "Be":
                        mapperConfiguration = new MapperConfiguration(cfg => 
                        {
                            cfg.CreateMap<Exhibit, ExhibitDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleBe))
                            .ForMember(destination => destination.Text,
                               map => map.Ignore())
                            .ForMember(destination => destination.Photos,
                            map => map.Ignore());
                            cfg.AllowNullCollections = true;
                        });
                        break;
                    default:
                        mapperConfiguration = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<Exhibit, ExhibitDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleEn))
                            .ForMember(destination => destination.Text,
                               map => map.Ignore())
                            .ForMember(destination => destination.Photos,
                            map => map.Ignore());
                            cfg.AllowNullCollections = true;
                        });
                        break;
                }
                var mapper = new Mapper(mapperConfiguration);
                exhibitsDTO = mapper.Map<List<ExhibitDTO>>(exhibits);
                if (exhibitsDTO.FirstOrDefault(el => string.IsNullOrEmpty(el.Title)) != null)
                {
                    throw new Error(Errors.Not_found, $"There is no title in {language} language");
                }
            }
            return exhibitsDTO;
        }

        public async Task<ExhibitDTO> GetAsync(HttpRequest request, string hallId, string standId, string id)
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
            var exhibit = await _exhibitsRepository.GetAsync(hallId, standId, id);
            MapperConfiguration mapperConfiguration = null;
            ExhibitDTO exhibitDTO = null;
            if (exhibit != null)
            {
                switch (language)
                {
                    case "Ru":
                        mapperConfiguration = new MapperConfiguration(cfg => 
                        {
                            cfg.CreateMap<Exhibit, ExhibitDTO>()
                            .ForMember(destination => destination.Title,
                                    map => map.MapFrom(
                                source => source.TitleRu))
                            .ForMember(destination => destination.Text,
                                    map => map.MapFrom(
                                source => source.TextRu));
                            cfg.AllowNullCollections = true;
                        }
                        );
                        break;
                    case "En":
                        mapperConfiguration = new MapperConfiguration(cfg => 
                        {
                            cfg.CreateMap<Exhibit, ExhibitDTO>()
                           .ForMember(destination => destination.Title,
                                   map => map.MapFrom(
                               source => source.TitleEn))
                           .ForMember(destination => destination.Text,
                                   map => map.MapFrom(
                               source => source.TextEn));
                            cfg.AllowNullCollections = true;
                        });
                        break;
                    case "Be":
                        mapperConfiguration = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<Exhibit, ExhibitDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleBe))
                            .ForMember(destination => destination.Text,
                                map => map.MapFrom(
                            source => source.TextBe));
                            cfg.AllowNullCollections = true;
                        });
                        break;
                    default:
                        mapperConfiguration = new MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<Exhibit, ExhibitDTO>()
                           .ForMember(destination => destination.Title,
                                   map => map.MapFrom(
                               source => source.TitleEn))
                           .ForMember(destination => destination.Text,
                                   map => map.MapFrom(
                               source => source.TextEn));
                            cfg.AllowNullCollections = true;
                        });
                        break;
                }
                var mapper = new Mapper(mapperConfiguration);
                exhibitDTO = mapper.Map<ExhibitDTO>(exhibit);

                if (string.IsNullOrEmpty(exhibitDTO.Text))
                {
                    throw new Error(Errors.Not_found, $"There is no text in {language} language");
                }
                if (string.IsNullOrEmpty(exhibitDTO.Title))
                {
                    throw new Error(Errors.Not_found, $"There is no title in {language} language");
                }
            }
            return exhibitDTO;
        }
    }
}
