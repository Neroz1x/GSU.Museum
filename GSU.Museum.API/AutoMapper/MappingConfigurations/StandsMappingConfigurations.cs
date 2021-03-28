using AutoMapper;
using GSU.Museum.CommonClassLibrary.Models;

namespace GSU.Museum.API.AutoMapper.MappingConfigurations
{
    public class StandsMappingConfigurations
    {
        public static MapperConfiguration GetAllRuConfiguration => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Stand, StandDTO>()
            .ForMember(destination => destination.Title,
                map => map.MapFrom(
                    source => source.TitleRu))
            .ForMember(destination => destination.Description,
                map => map.MapFrom(
                    source => source.DescriptionRu))
            .ForMember(destination => destination.Exhibits,
                map => map.Ignore())
            .ForMember(destination => destination.Photo,
                map => map.Ignore());
            cfg.AllowNullCollections = true;
        });

        public static MapperConfiguration GetAllRuPhotoConfiguration => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PhotoInfo, PhotoInfoDTO>()
            .ForMember(destination => destination.Description,
                map => map.MapFrom(
                    source => source.DescriptionRu));
        });

        public static MapperConfiguration GetAllEnConfiguration => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Stand, StandDTO>()
            .ForMember(destination => destination.Title,
                map => map.MapFrom(
            source => source.TitleEn))
            .ForMember(destination => destination.Description,
                map => map.MapFrom(
                    source => source.DescriptionEn))
            .ForMember(destination => destination.Exhibits,
                map => map.Ignore())
            .ForMember(destination => destination.Photo,
                map => map.Ignore());
            cfg.AllowNullCollections = true;
        });

        public static MapperConfiguration GetAllEnPhotoConfiguration => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PhotoInfo, PhotoInfoDTO>()
            .ForMember(destination => destination.Description,
                map => map.MapFrom(
                    source => source.DescriptionEn));
        });

        public static MapperConfiguration GetAllByConfiguration => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Stand, StandDTO>()
            .ForMember(destination => destination.Title,
                map => map.MapFrom(
                    source => source.TitleBe))
            .ForMember(destination => destination.Description,
                map => map.MapFrom(
                    source => source.DescriptionBe))
            .ForMember(destination => destination.Exhibits,
                map => map.Ignore())
            .ForMember(destination => destination.Photo,
                map => map.Ignore());
            cfg.AllowNullCollections = true;
        });

        public static MapperConfiguration GetAllByPhotoConfiguration => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PhotoInfo, PhotoInfoDTO>()
            .ForMember(destination => destination.Description,
                map => map.MapFrom(
                    source => source.DescriptionBe));
        });

        public static MapperConfiguration GetRuConfiguration => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Stand, StandDTO>()
            .ForMember(destination => destination.Title,
                map => map.MapFrom(
                    source => source.TitleRu))
            .ForMember(destination => destination.Description,
                map => map.MapFrom(
                    source => source.DescriptionRu))
            .ForMember(destination => destination.Exhibits,
                map => map.Ignore())
            .ForMember(destination => destination.Photo,
                map => map.Ignore());
            cfg.AllowNullCollections = true;
        });

        public static MapperConfiguration GetEnConfiguration => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Stand, StandDTO>()
            .ForMember(destination => destination.Title,
                map => map.MapFrom(
                    source => source.TitleEn))
            .ForMember(destination => destination.Description,
                map => map.MapFrom(
                    source => source.DescriptionEn))
            .ForMember(destination => destination.Exhibits,
                map => map.Ignore())
            .ForMember(destination => destination.Photo,
                map => map.Ignore());
            cfg.AllowNullCollections = true;
        });

        public static MapperConfiguration GetByConfiguration => new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Stand, StandDTO>()
            .ForMember(destination => destination.Title,
                map => map.MapFrom(
                    source => source.TitleBe))
            .ForMember(destination => destination.Description,
                map => map.MapFrom(
                    source => source.DescriptionBe))
            .ForMember(destination => destination.Exhibits,
                map => map.Ignore())
            .ForMember(destination => destination.Photo,
                map => map.Ignore());
            cfg.AllowNullCollections = true;
        });
    }
}
