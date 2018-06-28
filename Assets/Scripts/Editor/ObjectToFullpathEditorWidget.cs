using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TSAttributes
{
    [CustomPropertyDrawer(typeof(AssetFullPathGetterAttribute))]
    public class ObjectToFullpathEditorWidget : PropertyDrawer
    {
        protected const string RESET_BTN_TEXT = "reset";

        protected Rect resetBtnRect;
        protected Rect pathLabelRect;
        protected float defaultLineHeight;
        protected string path;

        public override void OnGUI(Rect initialRect, SerializedProperty property, GUIContent label)
        {
            defaultLineHeight = base.GetPropertyHeight(property, label);
            label.text = "";

            EditorGUI.BeginProperty(initialRect, label, property);
            EditorGUI.PrefixLabel(initialRect, GUIUtility.GetControlID(FocusType.Passive), label);

            path = property.stringValue;

            initialRect.height = defaultLineHeight;

            resetBtnRect = new Rect(initialRect.x, initialRect.y + defaultLineHeight, GUI.skin.label.CalcSize(new GUIContent(RESET_BTN_TEXT + 50)).x, initialRect.height);
            pathLabelRect = new Rect(initialRect.x + GUI.skin.label.CalcSize(new GUIContent(label)).x, initialRect.y, initialRect.width, initialRect.height);

            DrawThings(ref initialRect);

            property.stringValue = path;

            EditorGUI.LabelField(pathLabelRect, path);
            EditorGUI.EndProperty();
        }

        protected virtual void DrawThings(ref Rect initialRect)
        {
            if (string.IsNullOrEmpty(path))
            {
                SetPath(ref path, ref initialRect, ref defaultLineHeight);
            }
            else if (GUI.Button(resetBtnRect, RESET_BTN_TEXT))
            {
                path = string.Empty;
            }
        }

        protected virtual void SetPath(ref string path, ref Rect initialRect, ref float defaultLineHeight)
        {
            Object go = null;
            var objectInputFieldPos = new Rect(initialRect.x, initialRect.y + defaultLineHeight, initialRect.width, initialRect.height);
            go = EditorGUI.ObjectField(objectInputFieldPos, "Obj:", go, typeof(Object), true);
            path = AssetDatabase.GetAssetPath(go);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 3;
        }
    }
}
