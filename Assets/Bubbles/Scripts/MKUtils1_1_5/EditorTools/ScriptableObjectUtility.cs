using System.IO;
using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

/*
  27.10.2020
 */
namespace Mkey
{
    public static class ScriptableObjectUtility //http://wiki.unity3d.com/index.php?title=CreateScriptableObjectAsset
    {
#if UNITY_EDITOR
        /// <summary>
        //	This makes it easy to create, name and place unique new ScriptableObject asset files.
        /// </summary>
        public static T CreateAsset<T>(string subFolder, string namePrefix, string nameSuffics) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/Resources/" + subFolder + "/" + namePrefix + typeof(T).ToString().Replace('.', '_') + nameSuffics + ".asset");
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            T aT = (T)AssetDatabase.LoadAssetAtPath(assetPathAndName, typeof(T));
            Selection.activeObject = aT;
            return aT;
        }

        /// <summary>
        //	This makes it easy to create, name and place unique new ScriptableObject asset files in Resource/Subfolder .
        //  string path like "Assets/ForestMatch/Resources/";
        /// </summary>
        public static T CreateResourceAsset<T>(string path, string subFolder, string namePrefix, string nameSuffics) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + subFolder + "/" + namePrefix + typeof(T).ToString().Replace('.', '_') + nameSuffics + ".asset");
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            T aT = (T)AssetDatabase.LoadAssetAtPath(assetPathAndName, typeof(T));
            Selection.activeObject = aT;
            return aT;
        }

        /// <summary>
        //	This makes it easy to create, name and place unique new ScriptableObject asset files in Resource/Subfolder .
        //  string path like "Assets/ForestMatch/Resources/";
        /// </summary>
        public static T CreateResourceAsset<T>(string path, string subFolder, string name, string namePrefix, string nameSuffics) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + subFolder + "/" + namePrefix + name + nameSuffics + ".asset");
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            T aT = (T)AssetDatabase.LoadAssetAtPath(assetPathAndName, typeof(T));
            Selection.activeObject = aT;
            return aT;
        }

        /// <summary>
        //	This makes it easy to create, name and place unique new ScriptableObject asset files in Resource/Subfolder .
        /// </summary>
        public static void DeleteResourceAsset(UnityEngine.Object o)
        {
            string path = AssetDatabase.GetAssetPath(o);
            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            EditorUtility.FocusProjectWindow();
        }
#endif
    }
}