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
            // Get language from header
            StringValues language = LanguageConstants.LanguageDefault;
            request?.Headers?.TryGetValue("Accept-Language", out language);

            var halls = await _hallsRepository.GetAllAsync();
            MapperConfiguration mapperConfiguration = null;
            MapperConfiguration mapperConfigurationPhoto = null;
            List<HallDTO> hallsDTO = new List<HallDTO>();
            if (halls != null)
            {
                // Create mapping depending on language
                switch (language)
                {
                    case LanguageConstants.LanguageRu:
                        mapperConfiguration = HallsMappingConfigurations.GetAllRuConfiguration;
                        mapperConfigurationPhoto = HallsMappingConfigurations.GetAllRuPhotoConfiguration;
                        break;
                    case LanguageConstants.LanguageEn:
                        mapperConfiguration = HallsMappingConfigurations.GetAllEnConfiguration;
                        mapperConfigurationPhoto = HallsMappingConfigurations.GetAllEnPhotoConfiguration;
                        break;
                    case LanguageConstants.LanguageBy:
                        mapperConfiguration = HallsMappingConfigurations.GetAllByConfiguration;
                        mapperConfigurationPhoto = HallsMappingConfigurations.GetAllByPhotoConfiguration;
                        break;
                    default:
                        mapperConfiguration = HallsMappingConfigurations.GetAllEnConfiguration;
                        mapperConfigurationPhoto = HallsMappingConfigurations.GetAllEnPhotoConfiguration;
                        break;
                }
            }
            var mapper = new Mapper(mapperConfiguration);
            hallsDTO = mapper.Map<List<HallDTO>>(halls);

            mapper = new Mapper(mapperConfigurationPhoto);
            for (int i = 0; i < halls.Count; i++)
            {
                var photoInfoDTO = mapper.Map<PhotoInfoDTO>(halls[i].Photo);
                hallsDTO[i].Photo = photoInfoDTO;
            }
            
            return hallsDTO;
        }

        public async Task<HallDTO> GetAsync(HttpRequest request, string id)
        {
            // Get language from header
            StringValues language = LanguageConstants.LanguageDefault;
            request?.Headers?.TryGetValue("Accept-Language", out language);

            var hall = await _hallsRepository.GetAsync(id);
            MapperConfiguration mapperConfiguration = null;
            HallDTO hallDTO = null;
            if (hall != null)
            {
                // Create mapping depending on language
                switch (language)
                {
                    case LanguageConstants.LanguageRu:
                        mapperConfiguration = HallsMappingConfigurations.GetRuConfiguration;
                        break;
                    case LanguageConstants.LanguageEn:
                        mapperConfiguration = HallsMappingConfigurations.GetEnConfiguration;
                        break;
                    case LanguageConstants.LanguageBy:
                        mapperConfiguration = HallsMappingConfigurations.GetByConfiguration;
                        break;
                    default:
                        mapperConfiguration = HallsMappingConfigurations.GetEnConfiguration;
                        break;
                }
                var mapper = new Mapper(mapperConfiguration);
                hallDTO = mapper.Map<HallDTO>(hall);
                
                var stands = await _standsService.GetAllAsync(request, id);
                hallDTO.Stands = stands;
            }
            return hallDTO;
        }
    }
}
