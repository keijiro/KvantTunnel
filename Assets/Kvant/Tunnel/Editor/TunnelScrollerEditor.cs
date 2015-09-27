//
// Custom editor for TunnelScroller
//
using UnityEngine;
using UnityEditor;

namespace Kvant
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(TunnelScroller))]
    public class TunnelScrollerEditor : Editor
    {
        SerializedProperty _speed;

        void OnEnable()
        {
            _speed = serializedObject.FindProperty("_speed");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_speed);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
