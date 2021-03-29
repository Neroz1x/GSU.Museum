using AutoMapper;
using GSU.Museum.API.AutoMapper.MappingConfigurations;
using GSU.Museum.API.Interfaces;
using GSU.Museum.CommonClassLibrary.Constants;
using GSU.Museum.CommonClassLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
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
            // Get language from header
            StringValues language = LanguageConstants.LanguageDefault;
            request?.Headers?.TryGetValue("Accept-Language", out language);

            var stands = await _standsRepository.GetAllAsync(hallId);
            MapperConfiguration mapperConfiguration = null;
            MapperConfiguration mapperConfigurationPhoto = null;
            List<StandDTO> standsDTO = new List<StandDTO>();
            if (stands != null)
            {
                // Create mapping depending on language
                switch (language)
                {
                    case LanguageConstants.LanguageRu:
                        mapperConfiguration = StandsMappingConfigurations.GetAllRuConfiguration;
                        mapperConfigurationPhoto = StandsMappingConfigurations.GetAllRuPhotoConfiguration;
                        break;
                    case LanguageConstants.LanguageEn:
                        mapperConfiguration = StandsMappingConfigurations.GetAllEnConfiguration;
                        mapperConfigurationPhoto = StandsMappingConfigurations.GetAllEnPhotoConfiguration;
                        break;
                    case LanguageConstants.LanguageBy:
                        mapperConfiguration = StandsMappingConfigurations.GetAllByConfiguration;
                        mapperConfigurationPhoto = StandsMappingConfigurations.GetAllByPhotoConfiguration;
                        break;
                    default:
                        mapperConfiguration = StandsMappingConfigurations.GetAllEnConfiguration;
                        mapperConfigurationPhoto = StandsMappingConfigurations.GetAllEnPhotoConfiguration;
                        break;
                }
                var mapper = new Mapper(mapperConfiguration);
                standsDTO = mapper.Map<List<StandDTO>>(stands);
                mapper = new Mapper(mapperConfigurationPhoto);
                for(int i = 0; i < stands.Count; i++)
                {
                    var photoInfoDTO = mapper.Map<PhotoInfoDTO>(stands[i].Photo);
                    standsDTO[i].Photo = photoInfoDTO;
                }
                return standsDTO;
            }
            return null;
        }

        public async Task<StandDTO> GetAsync(HttpRequest request, string hallId, string id)
        {
            // Get language from header
            StringValues language = LanguageConstants.LanguageDefault;
            request?.Headers?.TryGetValue("Accept-Language", out language);

            var stand = await _standsRepository.GetAsync(hallId, id);
            MapperConfiguration mapperConfiguration = null;
            StandDTO standDTO = null;
            if (stand != null)
            {
                // Create mapping depending on language
                switch (language)
                {
                    case LanguageConstants.LanguageRu:
                        mapperConfiguration = StandsMappingConfigurations.GetRuConfiguration;
                        break;
                    case LanguageConstants.LanguageEn:
                        mapperConfiguration = StandsMappingConfigurations.GetEnConfiguration;
                        break;
                    case LanguageConstants.LanguageBy:
                        mapperConfiguration = StandsMappingConfigurations.GetByConfiguration;
                        break;
                    default:
                        mapperConfiguration = StandsMappingConfigurations.GetEnConfiguration;
                        break;
                }
                var mapper = new Mapper(mapperConfiguration);
                standDTO = mapper.Map<StandDTO>(stand);

                var exhibits = await _exhibitsService.GetAllAsync(request, hallId, id);
                standDTO.Exhibits = exhibits;
            }
            return standDTO;
        }
    }
}
