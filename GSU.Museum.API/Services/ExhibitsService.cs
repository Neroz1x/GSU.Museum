﻿using AutoMapper;
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
                        break;
                    case "En":
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
                        break;
                    case "Be":
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
                        break;
                }
                var mapper = new Mapper(mapperConfiguration);
                exhibitsDTO = mapper.Map<List<ExhibitDTO>>(exhibits);
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
            MapperConfiguration mapperConfigurationPhoto = null;

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
                            .ForMember(destination => destination.Description,
                                map => map.MapFrom(
                            source => source.DescriptionRu))
                            .ForMember(destination => destination.Text,
                                map => map.MapFrom(
                                    source => source.TextRu))
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
                    case "En":
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
                    case "Be":
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
