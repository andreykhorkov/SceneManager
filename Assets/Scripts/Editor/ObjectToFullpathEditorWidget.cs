using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(AssetFullPathGetterAttribute))]
public class ObjectToFullpathEditorWidget : PropertyDrawer
{
    private const string RESET_BTN_TEXT = "reset";
    private const string LOAD_SCENE_BTN_TEXT = "load";

    public override void OnGUI(Rect initialRect, SerializedProperty property, GUIContent label)
    {
        var t = property.serializedObject.targetObject as SceneDefinitions;
        Debug.Log(t.LoadableScenes);

        var defaultLineHeight = base.GetPropertyHeight(property, label);
        label.text = "";

        EditorGUI.BeginProperty(initialRect, label, property);
        EditorGUI.PrefixLabel(initialRect, GUIUtility.GetControlID(FocusType.Passive), label);

        var path = property.stringValue;

        initialRect.height = defaultLineHeight;

        var resetBtnRect = new Rect(initialRect.x, initialRect.y + defaultLineHeight, GUI.skin.label.CalcSize(new GUIContent(RESET_BTN_TEXT + 50)).x, initialRect.height);
        var loadSceneBtnRect = new Rect(initialRect.x + GUI.skin.label.CalcSize(new GUIContent(LOAD_SCENE_BTN_TEXT + 100)).x, initialRect.y + defaultLineHeight, GUI.skin.label.CalcSize(new GUIContent(LOAD_SCENE_BTN_TEXT + 50)).x, initialRect.height);
        var pathLabelRect = new Rect(initialRect.x + GUI.skin.label.CalcSize(new GUIContent(label)).x, initialRect.y, initialRect.width, initialRect.height);

        if (string.IsNullOrEmpty(path))
        {
            SetPath(ref path, ref initialRect, ref defaultLineHeight);
        }
        else if (GUI.Button(resetBtnRect, RESET_BTN_TEXT))
        {
            path = string.Empty;
        }
        else if (GUI.Button(loadSceneBtnRect, LOAD_SCENE_BTN_TEXT))
        {
            var scenesArr = new Scene[SceneManager.sceneCount];

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                scenesArr[i] = SceneManager.GetSceneAt(i);
            }

            EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);

            for (int i = 0; i < scenesArr.Length; i++)
            {
                SceneManager.UnloadSceneAsync(scenesArr[i]);
            }

            var myDataClass = PropertyDrawerUtility.GetActualObjectForSerializedProperty<SceneDefinitions>(fieldInfo, property);
        }

        property.stringValue = path;

        EditorGUI.LabelField(pathLabelRect, path);
        EditorGUI.EndProperty();
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

public class PropertyDrawerUtility
{
    public static T GetActualObjectForSerializedProperty<T>(FieldInfo fieldInfo, SerializedProperty property) where T : class
    {
        var obj = fieldInfo.GetValue(property.serializedObject.targetObject);
        if (obj == null) { return null; }

        T actualObject = null;
        if (obj.GetType().IsArray)
        {
            var index = Convert.ToInt32(new string(property.propertyPath.Where(c => char.IsDigit(c)).ToArray()));
            actualObject = ((T[])obj)[index];
        }
        else
        {
            actualObject = obj as T;
        }
        return actualObject;
    }
}
