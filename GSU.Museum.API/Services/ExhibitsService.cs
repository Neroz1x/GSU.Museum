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
    public class ExhibitsService : IExhibitsService
    {
        private readonly IExhibitsRepository _exhibitsRepository;

        public ExhibitsService(IExhibitsRepository exhibitsRepository)
        {
            _exhibitsRepository = exhibitsRepository;
        }

        public async Task<List<ExhibitDTO>> GetAllAsync(StringValues language, string hallId, string standId)
        {
            var exhibis = await _exhibitsRepository.GetAllAsync(hallId, standId);
            MapperConfiguration mapperConfiguration = null;
            List<ExhibitDTO> exhibitsDTO = new List<ExhibitDTO>();
            if (exhibis != null)
            {
                switch (language)
                {
                    case "Ru":
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Exhibit, ExhibitDTO>()
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleRu))
                        .ForMember(destination => destination.Text,
                               map => map.Ignore())
                        .ForMember(destination => destination.Photos,
                            map => map.Ignore())
                        );
                        break;
                    case "En":
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Exhibit, ExhibitDTO>()
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleEn))
                        .ForMember(destination => destination.Text,
                               map => map.Ignore())
                        .ForMember(destination => destination.Photos,
                            map => map.Ignore())
                        );
                        break;
                    case "Be":
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Exhibit, ExhibitDTO>()
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleBe))
                        .ForMember(destination => destination.Text,
                               map => map.Ignore())
                        .ForMember(destination => destination.Photos,
                            map => map.Ignore())
                        );
                        break;
                    default:
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Exhibit, ExhibitDTO>()
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleEn))
                        .ForMember(destination => destination.Text,
                               map => map.Ignore())
                        .ForMember(destination => destination.Photos,
                            map => map.Ignore())
                        );
                        break;
                }
                var mapper = new Mapper(mapperConfiguration);
                exhibitsDTO = mapper.Map<List<ExhibitDTO>>(exhibis);
                if (exhibitsDTO.First(el => string.IsNullOrEmpty(el.Text)) != null)
                {
                    throw new Error(Errors.Not_found, $"There is no text in {language} language");
                }
                if (exhibitsDTO.First(el => string.IsNullOrEmpty(el.Title)) != null)
                {
                    throw new Error(Errors.Not_found, $"There is no title in {language} language");
                }
            }
            return exhibitsDTO;
        }

        public async Task<ExhibitDTO> GetAsync(StringValues language, string hallId, string standId, string id)
        {
            var exhibit = await _exhibitsRepository.GetAsync(hallId, standId, id);
            MapperConfiguration mapperConfiguration = null;
            ExhibitDTO exhibitDTO = new ExhibitDTO();
            if (exhibit != null)
            {
                switch (language)
                {
                    case "Ru":
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Exhibit, ExhibitDTO>()
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleRu))
                        .ForMember(destination => destination.Text,
                                map => map.MapFrom(
                            source => source.TextRu))
                        );
                        break;
                    case "En":
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Exhibit, ExhibitDTO>()
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleBe))
                        .ForMember(destination => destination.Text,
                                map => map.MapFrom(
                            source => source.TextBe))
                        );
                        break;
                    case "Be":
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Exhibit, ExhibitDTO>()
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleBe))
                        .ForMember(destination => destination.Text,
                                map => map.MapFrom(
                            source => source.TextBe))
                        );
                        break;
                    default:
                        mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Exhibit, ExhibitDTO>()
                        .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleBe))
                        .ForMember(destination => destination.Text,
                                map => map.MapFrom(
                            source => source.TextBe))
                        );
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
