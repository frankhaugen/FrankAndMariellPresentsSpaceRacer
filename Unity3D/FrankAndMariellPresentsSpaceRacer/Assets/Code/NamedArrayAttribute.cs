using UnityEngine;
using UnityEditor;

namespace Code
{
    public class NamedArrayAttribute : PropertyAttribute
    {
        public readonly string[] names;
        public NamedArrayAttribute(string[] names) { this.names = names; }
    }
    
    [CustomPropertyDrawer (typeof(NamedArrayAttribute))]public class NamedArrayDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            try {
                var pos = int.Parse(property.propertyPath.Split('[', ']')[1]);
                EditorGUI.ObjectField(rect, property, new GUIContent(((NamedArrayAttribute)attribute).names[pos]));
            } catch {
                EditorGUI.ObjectField(rect, property, label);
            }
        }
    }
}