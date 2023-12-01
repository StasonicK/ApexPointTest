using System;

namespace Infrastructure
{
    public interface ISceneLoader
    {
        // void Load(Scene nextScene);
        void Load(SceneId nextSceneId, Action<SceneId> onLoaded = null);
        void Load(SceneId nextSceneId, Action onLoaded = null);
    }
}