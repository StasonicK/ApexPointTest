using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using StaticData;
using UnityEngine;

namespace Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string LevelsPath = "StaticData/Levels";

        private Dictionary<SceneId, LevelStaticData> _levels;

        public void Load()
        {
            _levels = Resources
                .LoadAll<LevelStaticData>(LevelsPath)
                .ToDictionary(x => x.SceneId, x => x);
        }

        public LevelStaticData ForLevel(SceneId sceneId) =>
            _levels.TryGetValue(sceneId, out LevelStaticData staticData)
                ? staticData
                : null;
    }
}