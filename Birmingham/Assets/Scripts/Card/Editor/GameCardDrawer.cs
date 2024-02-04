using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Game.Tech;
using Game.Card;

[CustomPropertyDrawer(typeof(GameCard))]
public class GameCardDrawer : PropertyDrawer
{
    private bool _isFoldOut;
    private float _margin = 2;
    private float _line => EditorGUIUtility.singleLineHeight + _margin;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        _isFoldOut = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), _isFoldOut, new GUIContent(property.displayName, GetTooltip()), true);
        position.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.indentLevel++;
        if (_isFoldOut)
        {
            SerializedProperty cardType = property.FindPropertyRelative("_type");
            EditorGUI.PropertyField(position, cardType, new GUIContent("Type"));
            position.y += _line;

            CardType type = (CardType)cardType.intValue;
            switch (type)
            {
                case CardType.CommonLocation:
                    SerializedProperty city = property.FindPropertyRelative("_city");
                    EditorGUI.PropertyField(position, city, new GUIContent("City"));
                    break;
                case CardType.CommonResource:
                    SerializedProperty resource = property.FindPropertyRelative("_resource");
                    EditorGUI.PropertyField(position, resource, new GUIContent("Resource"), true);
                    break;
                // skip universal types
                default: break;
            }
        }
        EditorGUI.indentLevel--;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!_isFoldOut)
            return EditorGUIUtility.singleLineHeight;
        SerializedProperty cardType = property.FindPropertyRelative("_type");
        CardType type = (CardType)cardType.intValue;
        switch (type)
        {
            case CardType.CommonLocation:
                return _line * 3;
            case CardType.CommonResource:
                return _line * 2 + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_resource"), label, true);

            // universal types
            default:
                return _line * 2;
        }
    }

    private string GetTooltip()
    {
        if (fieldInfo == null)
            return string.Empty;

        var tooltipAttribute = System.Attribute.GetCustomAttribute(fieldInfo, typeof(TooltipAttribute)) as TooltipAttribute;
        return tooltipAttribute == null ? string.Empty : tooltipAttribute.tooltip;
    }
}
