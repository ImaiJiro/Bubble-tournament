using System;
using System.IO;
//TODO: Don't remove!
using Etourney.Enums;
using UnityEditor;
using UnityEngine;

namespace Etourney.Settings
{
    public class SettingsLoader
    {
        private const string EtourneyFolder = "Etourney";
        private const string SettingsETourneyName = "etourney-settings.asset";

        private const string AssetsFolder = "Assets";
        private const string ResourcesFolder = "Resources";

        private static string ResourcesFolderPath
        {
            get
            {
                return Path.Combine(AssetsFolder, ResourcesFolder);
            }
        }

        private static string EtourneyFolderPath
        {
            get
            {
                return Path.Combine(ResourcesFolderPath, EtourneyFolder);
            }
        }

        private static string FullPatchToAsset
        {
            get
            {
                return Path.Combine(AssetsFolder, ResourcesFolder, EtourneyFolder, SettingsETourneyName);
            }
        }

        private static string PatchToAsset
        {
            get
            {
                return Path.Combine(EtourneyFolder, SettingsETourneyName);
            }
        }

        public static EtourneySettings Settings
        {
            get
            {
#if UNITY_EDITOR
                try
                { 
                    var result =  AssetDatabase.LoadAssetAtPath<EtourneySettings>(FullPatchToAsset);

                    return result == null ? CreateSettingsForEditor() : result;
                }
                catch (Exception)
                {
                    return CreateSettingsForEditor();
                }
#else
                //var settings = Resources.Load<EtourneySettings>(PatchToAsset);
                var settings = Resources.Load<EtourneySettings>($"{EtourneyFolder}/{SettingsETourneyName}");

                if (settings == null)
                    settings = ScriptableObject.CreateInstance<EtourneySettings>();

                return settings;
#endif
            }
        }
#if UNITY_EDITOR
        private static EtourneySettings CreateSettingsForEditor()
        {
            AssetDatabase.DeleteAsset(FullPatchToAsset);

            var settings = ScriptableObject.CreateInstance<EtourneySettings>();

            if (!AssetDatabase.IsValidFolder(ResourcesFolderPath))
            {
                AssetDatabase.GUIDToAssetPath(AssetDatabase.CreateFolder(AssetsFolder, ResourcesFolder));
            }

            if (!AssetDatabase.IsValidFolder(EtourneyFolderPath))
            {
                AssetDatabase.GUIDToAssetPath(AssetDatabase.CreateFolder(ResourcesFolderPath, EtourneyFolder));
            }

            AssetDatabase.CreateAsset(settings, FullPatchToAsset);

            return settings;
        }
#endif
    }
}