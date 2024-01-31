using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Game.TechTree;

[CustomPropertyDrawer(typeof(TechTypeVector<>))]
public class TechTypeVectorDrawer : PropertyDrawer
{
    private bool _isFoldOut;
    private float _margin = 2;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        _isFoldOut = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), _isFoldOut, new GUIContent(property.name, GetTooltip()), true);
        position.y += EditorGUIUtility.singleLineHeight;

        // Indent the content of the property
        position = EditorGUI.IndentedRect(position);
        if (_isFoldOut)
        {
            // Get the serialized list property
            SerializedProperty listProperty = property.FindPropertyRelative("_array");
            listProperty.arraySize = System.Enum.GetValues(typeof(TechType)).Length;
            // Display each element of the list
            for (int i = 0; i < listProperty.arraySize; i++)
            {
                SerializedProperty elementProperty = listProperty.GetArrayElementAtIndex(i);
                EditorGUI.LabelField(new Rect(position.x, position.y, position.width / 2, EditorGUIUtility.singleLineHeight), new GUIContent(((TechType)i).ToString()));
                EditorGUI.PropertyField(new Rect(position.x + position.width / 2, position.y, position.width / 2, EditorGUIUtility.singleLineHeight), elementProperty, GUIContent.none);
                position.y += EditorGUIUtility.singleLineHeight + _margin;
            }
        }
        
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if(!_isFoldOut)
            return EditorGUIUtility.singleLineHeight;
        int lines = System.Enum.GetValues(typeof(TechType)).Length;
        return EditorGUIUtility.singleLineHeight * (lines + 1) + _margin * lines;
    }

    private string GetTooltip()
    {
        if(fieldInfo == null)
            return string.Empty;

        var tooltipAttribute = System.Attribute.GetCustomAttribute(fieldInfo, typeof(TooltipAttribute)) as TooltipAttribute;
        return tooltipAttribute == null ? string.Empty : tooltipAttribute.tooltip;
    }
}
