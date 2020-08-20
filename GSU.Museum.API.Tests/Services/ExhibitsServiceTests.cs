using AutoMapper;
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
    public class ExhibitsServiceTests
    {
        private readonly ExhibitsService _service;
        private readonly ExhibitsService _serviceEmptyRepo;
        private readonly List<Hall> _halls;
        private const string HallId = "123456789012345678901234";
        private const string StandId = "123456789012345678901111";
        private HttpRequest httpRequest;
        public ExhibitsServiceTests()
        {
            var mockRepo = new Mock<IExhibitsRepository>();
            var mockRepoEmpty = new Mock<IExhibitsRepository>();
            List<Hall> _hallsEmpty = new List<Hall>()
            {
                new Hall()
                {
                    Id = HallId,
                    TitleBe = "Be",
                    TitleEn = "En",
                    TitleRu = "Ru",
                    Stands = new List<Stand>()
                    {
                        new Stand()
                        {
                            Id = StandId,
                            TitleRu = "TitleRu",
                            TitleEn = "TitleEn",
                            TitleBe = "TitleBe",
                            DescriptionBe = "Be1",
                            DescriptionEn = "En",
                            DescriptionRu = "Ru1",
                            Exhibits = new List<Exhibit>()
                        }
                    }
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
                                },
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
                        }
                    }
                }
            };
            mockRepo.Setup(repo =>
                repo.GetAllAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((string hallId, string standId)
                => _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.FirstOrDefault(s => s.Id.Equals(standId)).Exhibits.ToList());

            mockRepoEmpty.Setup(repo =>
               repo.GetAllAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((string hallId, string standId)
                => _hallsEmpty.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.FirstOrDefault(s => s.Id.Equals(standId)).Exhibits.ToList());

            mockRepo.Setup(repo =>
               repo.GetAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((string hallId, string standId, string id)
                => _halls.FirstOrDefault(h => h.Id.Equals(hallId)).Stands.FirstOrDefault(s => s.Id.Equals(standId)).Exhibits?.Where(e => e.Id.Equals(id)).FirstOrDefault());


            mockRepo.SetupAllProperties();
            mockRepoEmpty.SetupAllProperties();

            _service = new ExhibitsService(mockRepo.Object);
            _serviceEmptyRepo = new ExhibitsService(mockRepoEmpty.Object);
        }

        [Fact]
        public async void GetAll_RepositoryConsistsOf2Records_ShouldReturnListOf2Records()
        {
            // Arrange
            const int expected = 2;

            // Act
            var list = await _service.GetAllAsync(httpRequest, HallId, StandId);

            // Assert
            Assert.Equal(expected, list.Count);
        }

        [Fact]
        public async void GetAll_RepositoryIsEmpty_ShouldReturnEmptyList()
        {
            // Act
            var list = await _serviceEmptyRepo.GetAllAsync(httpRequest, HallId, StandId);

            // Assert
            Assert.Empty(list);
        }

        [Fact]
        public async void GetAsync_RecordWithRequestedIdExists_ShouldReturnRequestedRecord()
        {
            // Arrange
            Exhibit exhibit = _halls.First().Stands.First().Exhibits.First();

            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Exhibit, ExhibitDTO>()
                .ForMember(destination => destination.Title,
                        map => map.MapFrom(
                    source => source.TitleEn))
                .ForMember(destination => destination.Text,
                        map => map.MapFrom(
                    source => source.TextEn))
                );
            var mapper = new Mapper(mapperConfiguration);
            var expected = mapper.Map<ExhibitDTO>(exhibit);

            // Act
            var actual = await _service.GetAsync(httpRequest, HallId, StandId, "123456789012345678901212");

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
                var actual = await _service.GetAsync(httpRequest, HallId, StandId, "1");
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
            ExhibitDTO expected = null;

            // Act
            var actual = await _service.GetAsync(httpRequest, HallId, StandId, id);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
