//
// Custom editor for Tunnel
//
using UnityEngine;
using UnityEditor;

namespace Kvant
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Tunnel))]
    public class TunnelEditor : Editor
    {
        SerializedProperty _slices;
        SerializedProperty _stacks;
        SerializedProperty _radius;
        SerializedProperty _height;
        SerializedProperty _offset;

        SerializedProperty _noiseRepeat;
        SerializedProperty _noiseFrequency;
        SerializedProperty _noiseDepth;
        SerializedProperty _noiseClampMin;
        SerializedProperty _noiseClampMax;
        SerializedProperty _noiseElevation;
        SerializedProperty _noiseWarp;
        SerializedProperty _noiseOffset;

        SerializedProperty _material;
        SerializedProperty _castShadows;
        SerializedProperty _receiveShadows;
        SerializedProperty _lineColor;

        SerializedProperty _debug;

        static GUIContent _textSlices    = new GUIContent("Slices (on equator)");
        static GUIContent _textStacks    = new GUIContent("Stacks (along Z)");
        static GUIContent _textRepeat    = new GUIContent("Repeat (on equator)");
        static GUIContent _textFrequency = new GUIContent("Frequency (along Z)");
        static GUIContent _textDepth     = new GUIContent("Depth");
        static GUIContent _textClamp     = new GUIContent("Clamp");
        static GUIContent _textElevation = new GUIContent("Elevation");
        static GUIContent _textWarp      = new GUIContent("Warp");
        static GUIContent _textOffset    = new GUIContent("Offset");

        void OnEnable()
        {
            _slices = serializedObject.FindProperty("_slices");
            _stacks = serializedObject.FindProperty("_stacks");
            _radius = serializedObject.FindProperty("_radius");
            _height = serializedObject.FindProperty("_height");
            _offset = serializedObject.FindProperty("_offset");

            _noiseRepeat    = serializedObject.FindProperty("_noiseRepeat");
            _noiseFrequency = serializedObject.FindProperty("_noiseFrequency");
            _noiseDepth     = serializedObject.FindProperty("_noiseDepth");
            _noiseClampMin  = serializedObject.FindProperty("_noiseClampMin");
            _noiseClampMax  = serializedObject.FindProperty("_noiseClampMax");
            _noiseElevation = serializedObject.FindProperty("_noiseElevation");
            _noiseWarp      = serializedObject.FindProperty("_noiseWarp");
            _noiseOffset    = serializedObject.FindProperty("_noiseOffset");

            _material       = serializedObject.FindProperty("_material");
            _castShadows    = serializedObject.FindProperty("_castShadows");
            _receiveShadows = serializedObject.FindProperty("_receiveShadows");
            _lineColor      = serializedObject.FindProperty("_lineColor");

            _debug = serializedObject.FindProperty("_debug");
        }

        public override void OnInspectorGUI()
        {
            var instance = target as Tunnel;

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_slices, _textSlices);
            EditorGUILayout.PropertyField(_stacks, _textStacks);

            if (!_stacks.hasMultipleDifferentValues) {
                var note = "Allocated: " + instance.stacks;
                EditorGUILayout.LabelField(" ", note, EditorStyles.miniLabel);
            }

            if (EditorGUI.EndChangeCheck())
                instance.NotifyConfigChange();

            EditorGUILayout.PropertyField(_radius);
            EditorGUILayout.PropertyField(_height);
            EditorGUILayout.PropertyField(_offset);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Fractal Noise", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_noiseRepeat, _textRepeat);
            EditorGUILayout.PropertyField(_noiseFrequency, _textFrequency);
            EditorGUILayout.PropertyField(_noiseDepth, _textDepth);
            MinMaxSlider(_textClamp, _noiseClampMin, _noiseClampMax, -1.0f, 1.0f);
            EditorGUILayout.PropertyField(_noiseElevation, _textElevation);
            EditorGUILayout.PropertyField(_noiseWarp, _textWarp);
            EditorGUILayout.PropertyField(_noiseOffset, _textOffset);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_material);
            EditorGUILayout.PropertyField(_castShadows);
            EditorGUILayout.PropertyField(_receiveShadows);
            EditorGUILayout.PropertyField(_lineColor);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_debug);

            serializedObject.ApplyModifiedProperties();
        }

        void MinMaxSlider(
            GUIContent label,
            SerializedProperty propMin, SerializedProperty propMax,
            float minLimit, float maxLimit)
        {
            var min = propMin.floatValue;
            var max = propMax.floatValue;

            EditorGUI.BeginChangeCheck();

            // Min-max slider.
            EditorGUILayout.MinMaxSlider(label, ref min, ref max, minLimit, maxLimit);

            var prevIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Float value boxes.
            var rect = EditorGUILayout.GetControlRect();
            rect.x += EditorGUIUtility.labelWidth;
            rect.width = (rect.width - EditorGUIUtility.labelWidth) / 2 - 2;

            if (EditorGUIUtility.wideMode)
            {
                EditorGUIUtility.labelWidth = 28;
                min = Mathf.Clamp(EditorGUI.FloatField(rect, "min", min), minLimit, max);
                rect.x += rect.width + 4;
                max = Mathf.Clamp(EditorGUI.FloatField(rect, "max", max), min, maxLimit);
                EditorGUIUtility.labelWidth = 0;
            }
            else
            {
                min = Mathf.Clamp(EditorGUI.FloatField(rect, min), minLimit, max);
                rect.x += rect.width + 4;
                max = Mathf.Clamp(EditorGUI.FloatField(rect, max), min, maxLimit);
            }

            EditorGUI.indentLevel = prevIndent;

            if (EditorGUI.EndChangeCheck()) {
                propMin.floatValue = min;
                propMax.floatValue = max;
            }
        }
    }
}
