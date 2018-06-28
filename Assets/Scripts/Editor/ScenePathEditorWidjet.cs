using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TSAttributes
{
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

        protected override void DrawThings(ref Rect initialRect)
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
                    for (int i = 0; i < SceneManager.sceneCount - 1; i++)
                    {
                        EditorSceneManager.CloseScene(SceneManager.GetSceneAt(i), true);
                    }

                    var compositeScene = SceneLoader.FindSceneRecursively(path, sceneDefinitions.LoadableScenes);

                    if (compositeScene != null)
                    {
                        SceneLoader.LoadSceneAndItsSubscenesEditor(compositeScene, sceneDefinitions);
                    }
                    else
                    {
                        Debug.LogError("Can't load scene " + path);
                    }
                }
            }
        }
    }
}
