using Infrastructure;
using StaticData;

namespace Services.StaticData
{
    public interface IStaticDataService : IService
    {
        void Load();

        LevelStaticData ForLevel(SceneId sceneId);
    }
}