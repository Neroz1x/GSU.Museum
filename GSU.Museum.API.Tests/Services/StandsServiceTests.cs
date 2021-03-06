﻿using AutoMapper;
using GSU.Museum.API.Interfaces;
using GSU.Museum.API.Services;
using GSU.Museum.CommonClassLibrary.Enums;
using GSU.Museum.CommonClassLibrary.Models;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GSU.Museum.API.Tests.Services
{
    public class StandsServiceTests
    {
        private readonly ExhibitsService _exhibitsService;
        private readonly ExhibitsService _exhibitsServiceEmptyRepo;
        private readonly StandsService _service;
        private readonly StandsService _serviceEmptyRepo;
        private readonly List<Hall> _halls;
        private readonly HttpRequest httpRequest;
        private const string HallId = "123456789012345678901234";

        public StandsServiceTests()
        {
            var mockRepo = new Mock<IStandsRepository>();
            var mockRepoEmpty = new Mock<IStandsRepository>();
            List<Hall> _hallsEmpty = new List<Hall>()
            {
                new Hall()
                {
                    Id = HallId,
                    TitleBe = "Be",
                    TitleEn = "En",
                    TitleRu = "Ru",
                    Stands = new List<Stand>()
                }
            };
            _halls = new List<Hall>()
            {
                new Hall()
                {
                    Id = "123456789012345678901234",
                    TitleBe = "Be",
                    TitleEn = "En",
                    TitleRu = "Ru",
                    Stands = new List<Stand>()
                    {
                        new Stand()
                        {
                            Id = "123456789012345678901111",
                            TitleRu = "TitleRu",
                            TitleEn = "TitleEn",
                            TitleBe = "TitleBe",
                            DescriptionBe = "Be1",
                            DescriptionEn = "En",
                            DescriptionRu = "Ru1",
                            Exhibits = new List<Exhibit>()
                            {
                                new Exhibit()
                                {
                                    Id = "123456789012345678901212",
                                    TitleRu = "TitleRu",
                                    TitleEn = "TitleEn",
                                    TitleBe = "TitleBe",
                                    TextBe = "Be1",
                                    TextEn = "En1",
                                    TextRu = "Ru1",
                                },
                                new Exhibit()
                                {
                                    Id = "123456789012345678901213",
                                    TitleRu = "TitleRu2",
                                    TitleEn = "TitleEn2",
                                    TitleBe = "TitleBe2",
                                    TextBe = "Be12",
                                    TextEn = "En2",
                                    TextRu = "Ru12",
                                }
                            }
                        },
                        new Stand()
                        {
                            Id = "123456789012345678901112",
                            TitleRu = "TitleRu",
                            TitleEn = "TitleEn",
                            TitleBe = "TitleBe",
                            DescriptionBe = "Be1",
                            DescriptionEn = "En",
                            DescriptionRu = "Ru1",
                            Exhibits = new List<Exhibit>()
                            {
                                new Exhibit()
                                {
                                    Id = "123456789012345678901313",
                                    TitleRu = "TitleRu3",
                                    TitleBe = "TitleBe3",
                                    TextBe = "Be13",
                                    TextEn = "En13",
                                    TextRu = "Ru13",
                                }
                            }
                        },
                        new Stand()
                        {
                            Id = "123456789012345678903111",
                            TitleRu = "TitleRu",
                            TitleEn = "TitleEn",
                            TitleBe = "TitleBe",
                            DescriptionBe = "Be1",
                            DescriptionEn = "En",
                            DescriptionRu = "Ru1",
                            Exhibits = new List<Exhibit>()
                            {
                                new Exhibit()
                                {
                                    Id = "123456789012345678901415",
                                    TitleRu = "TitleRu24",
                                    TitleEn = "TitleEn24",
                                    TitleBe = "TitleBe24",
                                    TextBe = "Be124",
                                    TextRu = "Ru124",
                                }
                            }
                        },
                    }
                }
            };
            mockRepo.Setup(repo =>
                repo.GetAllAsync(It.IsAny<string>())).ReturnsAsync((string hallId)
                => _halls.FirstOrDefault(h => h.Id.Equals(hallId))?.Stands.ToList());

            mockRepoEmpty.Setup(repo =>
               repo.GetAllAsync(It.IsAny<string>())).ReturnsAsync((string hallId)
                => _hallsEmpty.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.ToList());

            mockRepo.Setup(repo =>
               repo.GetAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((string hallId, string id)
                => _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.Where(s => s.Id.Equals(id)).FirstOrDefault());

            mockRepo.Setup(repo => repo.CreateAsync(It.IsAny<string>(), It.IsAny<Stand>()))
            .Callback((string hallId, Stand stand) =>
            {
                _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.Add(stand);
            });

            mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stand>()))
            .Callback((string hallId, string id, Stand standIn) =>
            {
                var stand = _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands
                .FirstOrDefault(s => s.Id.Equals(id));
                stand.DescriptionEn = standIn.DescriptionEn;
                stand.DescriptionRu = standIn.DescriptionRu;
                stand.DescriptionBe = standIn.DescriptionBe;
                stand.TitleBe = standIn.TitleBe;
                stand.TitleEn = standIn.TitleEn;
                stand.TitleRu = standIn.TitleRu;
            });

            mockRepo.Setup(repo => repo.RemoveAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Callback((string hallId, string id) =>
            {
                var stand = _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.FirstOrDefault(e => e.Id.Equals(id));
                _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.Remove(stand);
            });

            var mockRepoExhibit = new Mock<IExhibitsRepository>();
            var mockRepoEmptyExhibits = new Mock<IExhibitsRepository>();

            mockRepo.SetupAllProperties();
            mockRepoEmpty.SetupAllProperties();

            mockRepoExhibit.Setup(repo =>
                repo.GetAllAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((string hallId, string standId)
                => _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.FirstOrDefault(s => s.Id.Equals(standId)).Exhibits.ToList());

            mockRepoEmptyExhibits.Setup(repo =>
               repo.GetAllAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((string hallId, string standId)
                => _hallsEmpty.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.FirstOrDefault(s => s.Id.Equals(standId)).Exhibits.ToList());

            mockRepoExhibit.Setup(repo =>
               repo.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((string hallId, string standId, string id)
                => _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.FirstOrDefault(s => s.Id.Equals(standId)).Exhibits?.Where(e => e.Id.Equals(id)).FirstOrDefault());


            _exhibitsService = new ExhibitsService(mockRepoExhibit.Object);
            _service = new StandsService(mockRepo.Object, _exhibitsService);

            _exhibitsServiceEmptyRepo = new ExhibitsService(mockRepoEmptyExhibits.Object);
            _serviceEmptyRepo = new StandsService(mockRepoEmpty.Object, _exhibitsServiceEmptyRepo);
        }

        [Fact]
        public async void GetAll_RepositoryConsistsOf3Records_ShouldReturnListOf3Records()
        {
            // Arrange
            const int expected = 3;

            // Act
            var list = await _service.GetAllAsync(httpRequest, HallId);

            // Assert
            Assert.Equal(expected, list.Count);
        }

        [Fact]
        public async void GetAll_RecordDoesNotFound_ShouldReturnNull()
        {
            // Act
            var actual = await _service.GetAllAsync(httpRequest, "111111189012345678901313");

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public async void GetAll_RepositoryIsEmpty_ShouldReturnEmptyList()
        {
            // Act
            var list = await _serviceEmptyRepo.GetAllAsync(httpRequest, HallId);

            // Assert
            Assert.Empty(list);
        }

        [Fact]
        public async void GetAsync_RecordWithRequestedIdExists_ShouldReturnRequestedRecord()
        {
            // Arrange
            Stand stand = _halls.First().Stands.First();

            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Stand, StandDTO>());
            mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Stand, StandDTO>()
            .ForMember(destination => destination.Title,
                    map => map.MapFrom(
                source => source.TitleEn))
            .ForMember(destination => destination.Description,
                    map => map.MapFrom(
                source => source.DescriptionEn))
            .ForMember(destination => destination.Exhibits,
                map => map.Ignore())
            .ForMember(destination => destination.Photo,
                                map => map.Ignore()));

            MapperConfiguration mapperConfigurationPhoto = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PhotoInfo, PhotoInfoDTO>()
                .ForMember(destination => destination.Description,
                    map => map.MapFrom(
                        source => source.DescriptionEn));
            });

            var mapper = new Mapper(mapperConfiguration);
            var expected = mapper.Map<StandDTO>(stand);
            mapper = new Mapper(mapperConfigurationPhoto);
            var photoInfoDTO = mapper.Map<PhotoInfoDTO>(stand.Photo);
            expected.Photo = photoInfoDTO;
            // Act
            var actual = await _service.GetAsync(httpRequest, HallId, expected.Id);

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public async void GetAsync_RequestRecordByIncorrectId_ShouldReturnIncorrectIdError()
        {
            // Arrange
            const Errors errorCode = Errors.Invalid_input;
            const string message = "Incorrect id length";

            // Act
            try
            {
                var actual = await _service.GetAsync(httpRequest, HallId, "1");
            }
            catch (Error err)
            {
                // Assert
                Assert.Equal(errorCode, err.ErrorCode);
                Assert.Equal(message, err.Info);
            }
        }

        [Fact]
        public async void GetAsync_RecordDoesNotExist_ShouldReturnNull()
        {
            const string id = "111111112111111111111113";

            // Arrange
            StandDTO expected = null;

            // Act
            var actual = await _service.GetAsync(httpRequest, HallId, id);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
