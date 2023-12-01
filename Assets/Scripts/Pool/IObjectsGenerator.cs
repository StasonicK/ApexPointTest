using System.Collections.Generic;
using GameObjects;
using UnityEngine;

namespace Pool
{
    public interface IObjectsGenerator : IGameObject
    {
        Dictionary<GameObjectType, List<GameObject>> GameObjectsDictionary { get; }

        void Construct(int enemies1Count, int enemies2Count, float secondsBetweenSpawns,
            IObjectsContainer objectsContainer);

        bool TryGetObject(GameObjectType type, out GameObject result);
        void Reset();
        void On();
        void Off();
    }
}