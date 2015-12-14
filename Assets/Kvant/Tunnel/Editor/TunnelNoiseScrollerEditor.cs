//
// Custom editor for TunnelNoiseScroller
//
using UnityEngine;
using UnityEditor;

namespace Kvant
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(TunnelNoiseScroller))]
    public class TunnelNoiseScrollerEditor : Editor
    {
        SerializedProperty _speed;
        SerializedProperty _rotation;

        void OnEnable()
        {
            _speed = serializedObject.FindProperty("_speed");
            _rotation = serializedObject.FindProperty("_rotation");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_speed);
            EditorGUILayout.PropertyField(_rotation);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
