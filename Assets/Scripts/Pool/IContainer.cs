using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public interface IContainer : IGameObject
    {
        List<GameObject> List { get; }
        void Add(GameObject item);
        void Clear();
    }
}