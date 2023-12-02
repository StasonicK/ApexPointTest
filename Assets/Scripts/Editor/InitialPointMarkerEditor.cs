using GameObjects.Enemies;
using GameObjects.Tank;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(SpawnMarker))]
    public class InitialPointMarkerEditor : UnityEditor.Editor
    {
        private static float _radius = 0.5f;

        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(InitialPointMarker spawner, GizmoType gizmo)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(spawner.transform.position, _radius);
        }
    }
}