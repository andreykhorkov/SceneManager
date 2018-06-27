using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(AssetFullPathGetterAttribute))]
public class ObjectToFullpathEditorWidget : PropertyDrawer
{
    private const string RESET_BTN_TEXT = "reset";
    private const string LOAD_SCENE_BTN_TEXT = "load";

    public override void OnGUI(Rect initialRect, SerializedProperty property, GUIContent label)
    {
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
                EditorSceneManager.CloseScene(EditorSceneManager.GetSceneAt(i), true);
            }

            var sceneDefinitions = property.serializedObject.targetObject as SceneDefinitions;

            CompositeScene compositeScene = null;

            if (TryFindSceneRecursively(path, sceneDefinitions.LoadableScenes, out compositeScene))
            {
                LoadSceneAndItsChildren(compositeScene, sceneDefinitions);
            }
            else
            {
                Debug.LogError("Can't load scene " + path);
                return;
            }
        }

        property.stringValue = path;

        EditorGUI.LabelField(pathLabelRect, path);
        EditorGUI.EndProperty();
    }

    private bool TryFindSceneRecursively(string path, List<CompositeScene> scenes, out CompositeScene foundScene)
    {
        for (int i = 0; i < scenes.Count; i++)
        {
            var comp = scenes[i];

            if (string.Equals(path, comp.ScenePath))
            {
                foundScene = comp;
                return true;
            }
        }

        for (int i = 0; i < scenes.Count; i++)
        {
            var comp = scenes[i];

            if (TryFindSceneRecursively(path, comp.SubScenes, out foundScene))
            {
                return true;
            }
        }

        foundScene = null;
        return false;
    }

    private void LoadSceneAndItsChildren(CompositeScene parentCompositeScene, SceneDefinitions sceneDefinitions)
    {
        //load root scene additively
        EditorSceneManager.OpenScene(sceneDefinitions.RootScenePath, OpenSceneMode.Single);

        //unload previous scenes to avoid nullrefs

        EditorSceneManager.OpenScene(parentCompositeScene.ScenePath, OpenSceneMode.Additive);

        OpenChildrenRecursively(parentCompositeScene);
    }

    private void OpenChildrenRecursively(CompositeScene scene)
    {
        foreach (var child in scene.SubScenes)
        {
            EditorSceneManager.OpenScene(child.ScenePath, OpenSceneMode.Additive);
            OpenChildrenRecursively(child);
        }
    }

    private void IterateSubScenes(List<CompositeScene> subScenes, string scenePath)
    {
        for (int i = 0; i < subScenes.Count; i++)
        {
            if (string.Equals(subScenes[i].ScenePath, scenePath))
            {
                EditorSceneManager.OpenScene(subScenes[i].ScenePath, OpenSceneMode.Additive);
                IterateSubScenes(subScenes[i].SubScenes, subScenes[i].ScenePath);
                return;
            }

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
