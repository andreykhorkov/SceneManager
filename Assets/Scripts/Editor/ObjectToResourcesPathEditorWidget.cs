using UnityEditor;
using UnityEngine;

namespace TSAttributes
{
    [CustomPropertyDrawer(typeof(AssetResourcesPathGetterAttribute))]
    public class ObjectToResourcesPathEditorWidget : ObjectToFullpathEditorWidget
    {
        protected override void SetPath(ref string path, ref Rect initialRect, ref float defaultLineHeight)
        {
            base.SetPath(ref path, ref initialRect, ref defaultLineHeight);

            path = path.Replace(".prefab", string.Empty);
            path = path.Replace("Assets/Resources/", string.Empty);
        }
    }
}

