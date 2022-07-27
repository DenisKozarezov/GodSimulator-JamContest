using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Editor
{
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Field)]
    public sealed class MinMaxSliderAttribute : PropertyAttribute
    {
        public readonly float MinLimit;
        public readonly float MaxLimit;

        public MinMaxSliderAttribute(float minLimit, float maxLimit)
        {
            MinLimit = minLimit;
            MaxLimit = maxLimit;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
    internal sealed class MinMaxSliderAttributeDrawer : PropertyDrawer
    {
        private const float FloatWidth = 45f;
        private const float Padding = 8f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool isVector2Int = property.propertyType == SerializedPropertyType.Vector2Int;
            if (property.propertyType != SerializedPropertyType.Vector2 && !isVector2Int)
            {
                Debug.LogWarning("MinMaxSliderAttribute requires a Vector2 or Vector2Int property");
                return;
            }

            var rangeAttribute = (MinMaxSliderAttribute)attribute;

            EditorGUI.LabelField(position, new GUIContent(property.displayName, property.tooltip));

            position.x = EditorGUIUtility.labelWidth + 5f;
            position.height = EditorGUIUtility.singleLineHeight;
            
            Rect minRect = new Rect(position.x, position.y, FloatWidth, position.height);
            position.x += FloatWidth - Padding;
            
            float sliderWidth = EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth - FloatWidth * 2 + 5f;
            Rect sliderRect = new Rect(position.x, position.y, sliderWidth, position.height);
            
            position.x += sliderWidth - Padding;
            Rect maxRect = new Rect(position.x, position.y, FloatWidth, position.height);

            float min = isVector2Int ? property.vector2IntValue.x : property.vector2Value.x;
            float max = isVector2Int ? property.vector2IntValue.y : property.vector2Value.y;

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            min = EditorGUI.FloatField(minRect, min);
            max = EditorGUI.FloatField(maxRect, max);
            EditorGUI.MinMaxSlider(sliderRect, ref min, ref max, rangeAttribute.MinLimit, rangeAttribute.MaxLimit);
            if (EditorGUI.EndChangeCheck())
            {
                if (isVector2Int)
                    property.vector2IntValue = CalculateIntRange(min, max);

                else
                    property.vector2Value = new Vector2(min, max);

                property.serializedObject.ApplyModifiedProperties();
            }
            EditorGUI.EndProperty();
        }

        private Vector2Int CalculateIntRange(float min, float max)
        {
            return new Vector2Int(Mathf.FloorToInt(min), Mathf.FloorToInt(max));
        }
    }
#endif
}