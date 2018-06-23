using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AssetFullPathGetterAttribute))]
public class ObjectToFullpathEditorWidget : PropertyDrawer
{
    private const string RESET_BTN_TEXT = "reset";

    public override void OnGUI(Rect initialRect, SerializedProperty property, GUIContent label)
    {
        var defaultLineHeight = base.GetPropertyHeight(property, label);
        label.text += ": ";

        EditorGUI.BeginProperty(initialRect, label, property);
        EditorGUI.PrefixLabel(initialRect, GUIUtility.GetControlID(FocusType.Passive), label);

        var path = property.stringValue;

        initialRect.height = defaultLineHeight;

        var resetBtnRect = new Rect(initialRect.x, initialRect.y + defaultLineHeight, GUI.skin.label.CalcSize(new GUIContent(RESET_BTN_TEXT + 50)).x, initialRect.height);
        var pathLabelRect = new Rect(initialRect.x + GUI.skin.label.CalcSize(new GUIContent(label)).x, initialRect.y, initialRect.width, initialRect.height);

        if (string.IsNullOrEmpty(path))
        {
            SetPath(ref path, ref initialRect, ref defaultLineHeight);
        }
        else if (GUI.Button(resetBtnRect, "reset"))
        {
            path = string.Empty;
        }

        property.stringValue = path;

        EditorGUI.LabelField(pathLabelRect, path);
        EditorGUI.EndProperty();
    }

    protected virtual void SetPath(ref string path, ref Rect initialRect, ref float defaultLineHeight)
    {
        Object go = null;
        var objectInputFieldPos = new Rect(initialRect.x, initialRect.y + defaultLineHeight, initialRect.width, initialRect.height);
        go = EditorGUI.ObjectField(objectInputFieldPos, "Prefab:", go, typeof(Object), true);
        path = AssetDatabase.GetAssetPath(go);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) * 3;
    }
}
