using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomPropertyDrawer(typeof(ScenePathGetterAttribute))]
public class ScenePathEditorWidjet : ObjectToFullpathEditorWidget
{
    protected const string LOAD_SCENE_BTN_TEXT = "load";

    protected Rect loadSceneBtnRect;
    protected SceneDefinitions sceneDefinitions;

    public override void OnGUI(Rect initialRect, SerializedProperty property, GUIContent label)
    {
        loadSceneBtnRect = new Rect(initialRect.x + GUI.skin.label.CalcSize(new GUIContent(LOAD_SCENE_BTN_TEXT + 100)).x, initialRect.y + defaultLineHeight, GUI.skin.label.CalcSize(new GUIContent(LOAD_SCENE_BTN_TEXT + 50)).x, defaultLineHeight);
        sceneDefinitions = property.serializedObject.targetObject as SceneDefinitions;

        base.OnGUI(initialRect, property, label);
    }

    protected override void Temp(ref Rect initialRect)
    {
        if (string.IsNullOrEmpty(path))
        {
            SetPath(ref path, ref initialRect, ref defaultLineHeight);
        }
        else if (!string.Equals(path, sceneDefinitions.RootScenePath))
        {
            if (GUI.Button(resetBtnRect, RESET_BTN_TEXT))
            {
                path = string.Empty;
            }
            else if (GUI.Button(loadSceneBtnRect, LOAD_SCENE_BTN_TEXT))
            {
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    EditorSceneManager.CloseScene(SceneManager.GetSceneAt(i), true);
                }

                var compositeScene = TryFindSceneRecursively(path, sceneDefinitions.LoadableScenes);

                if (compositeScene != null)
                {
                    LoadSceneAndItsChildren(compositeScene, sceneDefinitions);
                }
                else
                {
                    Debug.LogError("Can't load scene " + path);
                    return;
                }
            }
        }
    }

    private static CompositeScene TryFindSceneRecursively(string path, List<CompositeScene> scenes)
    {
        for (int i = 0; i < scenes.Count; i++)
        {
            var compositeScene = scenes[i];

            if (string.Equals(path, compositeScene.ScenePath))
            {
                return compositeScene;
            }
        }

        for (int i = 0; i < scenes.Count; i++)
        {
            var compositeScene = scenes[i];

            var neededScene = TryFindSceneRecursively(path, compositeScene.SubScenes);

            if (neededScene != null)
            {
                return neededScene;
            }
        }

        return null;
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
}
