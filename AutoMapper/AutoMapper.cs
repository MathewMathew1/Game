using AutoMapper;
using BoardGameBackend.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserModel, Player>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Username));
        CreateMap<UserModel, PlayerInLobby>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Username))
            .ForMember(dest => dest.IsConnected, opt => opt.MapFrom(src => true));
        CreateMap<UserModel, UserModelDto>();
        CreateMap<PlayerInGame, PlayerViewModel>()
            .ForMember(dest => dest.ResourceManager, opt => opt.MapFrom(src => src.ResourceManager));
        
        CreateMap<ResourceManager, ResourceManagerViewModel>()
            .ForMember(dest => dest.Resources, opt => opt.MapFrom(src => src.GetResources()));
        CreateMap<HeroCardFromJson, HeroCard>()
            .ForMember(dest => dest.Faction, opt => opt.MapFrom(src => Fractions.GetFractionById(src.Faction)));

    }


}
namespace BoardGameBackend.Mappers
{
    public static class GameMapper
    {
        // Static instance of the IMapper
        private static IMapper _mapper;

        // Static constructor to initialize the mapper with the game-specific profiles
        static GameMapper()
        {
            // Define the mapping configuration
            var config = new MapperConfiguration(cfg =>
            {

                cfg.AddProfile<GameMappingProfile>();
            });

            _mapper = config.CreateMapper();
        }


        public static IMapper Instance => _mapper;
    }

    public class GameMappingProfile : Profile
    {
        public GameMappingProfile()
        {
             CreateMap<MoveOnTile, MoveOnTileForOtherUsers>()
            .ForMember(dest => dest.TileReward, opt => opt.MapFrom(src => src.TileReward));

            CreateMap<GetCurrentTileReward, GetCurrentTileRewardForOtherUsers>()
            .ForMember(dest => dest.TileReward, opt => opt.MapFrom(src => src.TileReward));

            CreateMap<HeroCardFromJson, HeroCard>()
                .ForMember(dest => dest.Faction, opt => opt.MapFrom(src => Fractions.GetFractionById(src.Faction)));
            CreateMap<MercenaryFromJson, Mercenary>()
            .ForMember(dest => dest.ResourcesNeeded, opt => opt.MapFrom(src => MapResourcesNeeded(src.ResourcesNeeded)))
            .ForMember(dest => dest.Faction, opt => opt.MapFrom(src => Fractions.GetFractionById(src.Faction)));

            CreateMap<ArtifactFromJson, Artifact>();
            CreateMap<RolayCardFromJson, RolayCard>().
            ForMember(dest => dest.Faction, opt => opt.MapFrom(src => Fractions.GetFractionById(src.FactionId)));
            
            CreateMap<ArtifactToPickFromData, ArtifactToPickFromDataForOtherUsers>()
                .ForMember(dest => dest.ArtifactsAmount, opt => opt.MapFrom(src => src.Artifacts.Count));

            CreateMap<ArtifactsTaken, ArtifactsTakenDataForOtherUsers>()
                .ForMember(dest => dest.ArtifactsAmount, opt => opt.MapFrom(src => src.Artifacts.Count));

            CreateMap<ArtifactRerolledData, ArtifactRerolledDataForOtherUsers>();

            CreateMap<ResourceJsonInfo, ResourceInfo>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => GetResourceTypeFromId(src.ResourceId)));

             CreateMap<TileReward, TileRewardForOtherUsers>()
            .ForMember(dest => dest.GotArtifact, opt => opt.MapFrom(src => src.Artifact != null)); 
            CreateMap<ResourceManager, ResourceManagerViewModel>()
            .ForMember(dest => dest.Resources, opt => opt.MapFrom(src => src.GetResources()));
            CreateMap<PlayerInGame, PlayerViewModel>()
            .ForMember(dest => dest.ResourceManager, opt => opt.MapFrom(src => src.ResourceManager));

             CreateMap<PlayerInGame, Player>();
        }

        private List<ResourceInfo> MapResourcesNeeded(List<ResourceJsonInfo> resourcesJson)
        {
            return resourcesJson.Select(resourceJson =>
            {
                var resourceInfoJson = ResourcesFactory.MercenariesFromJsonList
                    .FirstOrDefault(r => r.Id == resourceJson.ResourceId);

                if (resourceInfoJson == null)
                {
                    throw new Exception($"Resource with ID {resourceJson.ResourceId} not found.");
                }

                if (!Enum.TryParse<ResourceType>(resourceInfoJson.NameEng, out var resourceType))
                {
                    throw new Exception($"Invalid ResourceType for resource {resourceInfoJson.NameEng}");
                }

                return new ResourceInfo
                {
                    Name = resourceType,
                    Amount = resourceJson.Amount
                };
            }).ToList();
        }

        private ResourceType GetResourceTypeFromId(int resourceId)
        {
            var resourceInfoJson = ResourcesFactory.MercenariesFromJsonList
                .FirstOrDefault(r => r.Id == resourceId);

            if (resourceInfoJson == null)
            {
                throw new Exception($"Resource with ID {resourceId} not found.");
            }

            if (!Enum.TryParse<ResourceType>(resourceInfoJson.NameEng, out var resourceType))
            {
                throw new Exception($"Invalid ResourceType for resource {resourceInfoJson.NameEng}");
            }

            return resourceType;
        }
    }


}
