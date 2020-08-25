using AutoMapper;
using GSU.Museum.API.Interfaces;
using GSU.Museum.CommonClassLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
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
            // Get language from header
            StringValues language = "";
            if(request != null)
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
            var exhibits = await _exhibitsRepository.GetAllAsync(hallId, standId);
            MapperConfiguration mapperConfiguration = null;
            MapperConfiguration mapperConfigurationPhoto = null;

            List<ExhibitDTO> exhibitsDTO = new List<ExhibitDTO>();
            if (exhibits != null)
            {
                // Create mapping depending on language
                switch (language)
                {
                    case "ru":
                        mapperConfiguration = new MapperConfiguration(cfg => 
                        {
                            cfg.CreateMap<Exhibit, ExhibitDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleRu))
                            .ForMember(destination => destination.Description,
                                map => map.MapFrom(
                            source => source.DescriptionRu))
                            .ForMember(destination => destination.Text,
                               map => map.MapFrom(
                                   source => source.TextRu))
                            .ForMember(destination => destination.Photos,
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
                            cfg.CreateMap<Exhibit, ExhibitDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleEn))
                            .ForMember(destination => destination.Description,
                                map => map.MapFrom(
                            source => source.DescriptionEn))
                            .ForMember(destination => destination.Text,
                               map => map.MapFrom(
                                   source => source.TextEn))
                            .ForMember(destination => destination.Photos,
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
                            cfg.CreateMap<Exhibit, ExhibitDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleBe))
                            .ForMember(destination => destination.Description,
                                map => map.MapFrom(
                            source => source.DescriptionBe))
                            .ForMember(destination => destination.Text,
                               map => map.MapFrom(
                                   source => source.TextBe))
                            .ForMember(destination => destination.Photos,
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
                            cfg.CreateMap<Exhibit, ExhibitDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                            source => source.TitleEn))
                            .ForMember(destination => destination.Description,
                                map => map.MapFrom(
                            source => source.DescriptionEn))
                            .ForMember(destination => destination.Text,
                               map => map.MapFrom(
                                   source => source.TextEn))
                            .ForMember(destination => destination.Photos,
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
                exhibitsDTO = mapper.Map<List<ExhibitDTO>>(exhibits);

                mapper = new Mapper(mapperConfigurationPhoto);
                for(int i = 0; i < exhibits.Count; i++)
                {
                    var photoInfoDTO = mapper.Map<List<PhotoInfoDTO>>(exhibits[i].Photos);
                    exhibitsDTO[i].Photos = photoInfoDTO;
                }
            }
            return exhibitsDTO;
        }

        public async Task<ExhibitDTO> GetAsync(HttpRequest request, string hallId, string standId, string id)
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
            var exhibit = await _exhibitsRepository.GetAsync(hallId, standId, id);
            MapperConfiguration mapperConfiguration = null;
            MapperConfiguration mapperConfigurationPhoto = null;

            ExhibitDTO exhibitDTO = null;
            if (exhibit != null)
            {
                // Create mapping depending on language
                switch (language)
                {
                    case "ru":
                        mapperConfiguration = new MapperConfiguration(cfg => 
                        {
                            cfg.CreateMap<Exhibit, ExhibitDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                                    source => source.TitleRu))
                            .ForMember(destination => destination.Description,
                                map => map.MapFrom(
                            source => source.DescriptionRu))
                            .ForMember(destination => destination.Text,
                                map => map.MapFrom(
                                    source => source.TextRu))
                            .ForMember(destination => destination.ExhibitType,
                                map => map.MapFrom(
                                    source => source.ExhibitType))
                            .ForMember(destenation => destenation.Photos,
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
                            cfg.CreateMap<Exhibit, ExhibitDTO>()
                           .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                                    source => source.TitleEn))
                           .ForMember(destination => destination.Description,
                                map => map.MapFrom(
                            source => source.DescriptionEn))
                           .ForMember(destination => destination.Text,
                                map => map.MapFrom(
                                    source => source.TextEn))
                           .ForMember(destination => destination.ExhibitType,
                                map => map.MapFrom(
                                    source => source.ExhibitType))
                           .ForMember(destenation => destenation.Photos,
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
                            cfg.CreateMap<Exhibit, ExhibitDTO>()
                            .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                                    source => source.TitleBe))
                            .ForMember(destination => destination.Description,
                                map => map.MapFrom(
                            source => source.DescriptionBe))
                            .ForMember(destination => destination.Text,
                                map => map.MapFrom(
                                    source => source.TextBe))
                            .ForMember(destination => destination.ExhibitType,
                                map => map.MapFrom(
                                    source => source.ExhibitType))
                            .ForMember(destenation => destenation.Photos,
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
                            cfg.CreateMap<Exhibit, ExhibitDTO>()
                           .ForMember(destination => destination.Title,
                                map => map.MapFrom(
                                    source => source.TitleEn))
                           .ForMember(destination => destination.Description,
                                map => map.MapFrom(
                            source => source.DescriptionEn))
                           .ForMember(destination => destination.Text,
                                map => map.MapFrom(
                                    source => source.TextEn))
                           .ForMember(destination => destination.ExhibitType,
                                map => map.MapFrom(
                                    source => source.ExhibitType))
                           .ForMember(destenation => destenation.Photos,
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
                exhibitDTO = mapper.Map<ExhibitDTO>(exhibit);

                mapper = new Mapper(mapperConfigurationPhoto);
                var photoInfoDTO = mapper.Map<List<PhotoInfoDTO>>(exhibit.Photos);
                exhibitDTO.Photos = photoInfoDTO;
            }
            return exhibitDTO;
        }
    }
}
