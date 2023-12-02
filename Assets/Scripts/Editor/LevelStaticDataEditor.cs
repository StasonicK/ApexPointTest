using System.Linq;
using GameObjects.Enemies;
using StaticData;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        private const string TankInitialPointTag = "TankInitialPoint";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LevelStaticData levelData = (LevelStaticData)target;

            if (GUILayout.Button("Collect"))
            {
                SpawnMarker[] findObjectsOfType = FindObjectsOfType<SpawnMarker>();
                levelData.EnemySpawners =
                    findObjectsOfType
                        .Select(x => x.transform.position)
                        .ToList();

                if (levelData.GameLoopLevel)
                    levelData.InitialTankPosition = GameObject.FindWithTag(TankInitialPointTag).transform.position;
            }

            EditorUtility.SetDirty(target);
        }
    }
}