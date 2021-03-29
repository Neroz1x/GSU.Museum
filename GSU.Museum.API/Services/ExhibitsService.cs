using AutoMapper;
using GSU.Museum.API.Interfaces;
using GSU.Museum.CommonClassLibrary.Constants;
using GSU.Museum.CommonClassLibrary.Models;
using GSU.Museum.API.AutoMapper.MappingConfigurations;
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
            StringValues language = LanguageConstants.LanguageDefault;
            request?.Headers?.TryGetValue("Accept-Language", out language);

            var exhibits = await _exhibitsRepository.GetAllAsync(hallId, standId);
            MapperConfiguration mapperConfiguration = null;
            MapperConfiguration mapperConfigurationPhoto = null;

            List<ExhibitDTO> exhibitsDTO = new List<ExhibitDTO>();
            if (exhibits != null)
            {
                // Create mapping depending on language
                switch (language)
                {
                    case LanguageConstants.LanguageRu:
                        mapperConfiguration = ExhibitsMappingConfigurations.GetAllRuConfiguration;
                        mapperConfigurationPhoto = ExhibitsMappingConfigurations.GetAllRuPhotoConfiguration;
                        break;
                    case LanguageConstants.LanguageEn:
                        mapperConfiguration = ExhibitsMappingConfigurations.GetAllEnConfiguration;
                        mapperConfigurationPhoto = ExhibitsMappingConfigurations.GetAllEnPhotoConfiguration;
                        break;
                    case LanguageConstants.LanguageBy:
                        mapperConfiguration = ExhibitsMappingConfigurations.GetAllByConfiguration;
                        mapperConfigurationPhoto = ExhibitsMappingConfigurations.GetAllByPhotoConfiguration;
                        break;
                    default:
                        mapperConfiguration = ExhibitsMappingConfigurations.GetAllEnConfiguration;
                        mapperConfigurationPhoto = ExhibitsMappingConfigurations.GetAllEnPhotoConfiguration;
                        break;
                }
                var mapper = new Mapper(mapperConfiguration);
                exhibitsDTO = mapper.Map<List<ExhibitDTO>>(exhibits);

                mapper = new Mapper(mapperConfigurationPhoto);
                for(int i = 0; i < exhibits.Count; i++)
                {
                    if (exhibits[i]?.Photos?.Count != 0)
                    {
                        var photoInfoDTO = mapper.Map<PhotoInfoDTO>(exhibits[i]?.Photos[0]);
                        exhibitsDTO[i].Photos = new List<PhotoInfoDTO>();
                        exhibitsDTO[i].Photos.Add(photoInfoDTO);
                    }
                    else
                    {
                        exhibitsDTO[i].Photos = null;
                    }
                }
                return exhibitsDTO;
            }
            return null;
        }

        public async Task<ExhibitDTO> GetAsync(HttpRequest request, string hallId, string standId, string id)
        {
            // Get language from header
            StringValues language = LanguageConstants.LanguageDefault;
            request?.Headers?.TryGetValue("Accept-Language", out language);

            var exhibit = await _exhibitsRepository.GetAsync(hallId, standId, id);
            MapperConfiguration mapperConfiguration = null;
            MapperConfiguration mapperConfigurationPhoto = null;

            ExhibitDTO exhibitDTO = null;
            if (exhibit != null)
            {
                // Create mapping depending on language
                switch (language)
                {
                    case LanguageConstants.LanguageRu:
                        mapperConfiguration = ExhibitsMappingConfigurations.GetRuConfiguration;
                        mapperConfigurationPhoto = ExhibitsMappingConfigurations.GetRuPhotoConfiguration;
                        break;
                    case LanguageConstants.LanguageEn:
                        mapperConfiguration = ExhibitsMappingConfigurations.GetEnConfiguration;
                        mapperConfigurationPhoto = ExhibitsMappingConfigurations.GetEnPhotoConfiguration;
                        break;
                    case LanguageConstants.LanguageBy:
                        mapperConfiguration = ExhibitsMappingConfigurations.GetByConfiguration;
                        mapperConfigurationPhoto = ExhibitsMappingConfigurations.GetByPhotoConfiguration;
                        break;
                    default:
                        mapperConfiguration = ExhibitsMappingConfigurations.GetEnConfiguration;
                        mapperConfigurationPhoto = ExhibitsMappingConfigurations.GetEnPhotoConfiguration;
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
