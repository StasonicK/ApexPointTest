using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public class EnemiesContainer : MonoBehaviour, IEnemiesContainer
    {
        [HideInInspector] public List<GameObject> List { get; private set; }

        private void Awake()
        {
            List = new List<GameObject>();
            DontDestroyOnLoad(this);
        }

        public GameObject GameObject => gameObject;

        public void Add(GameObject item) =>
            List.Add(item);

        public void Clear()
        {
            for (int i = 0; i < List.Count; i++)
                Destroy(List[i]);
        }
    }
}