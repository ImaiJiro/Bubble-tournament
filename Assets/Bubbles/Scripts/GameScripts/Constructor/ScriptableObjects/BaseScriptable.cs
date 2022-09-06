using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

/*
    10.11.2020 
 */
namespace Mkey
{
    public class BaseScriptable : ScriptableObject
    {
        public override string ToString()
        {
            return name + "; id: " + GetInstanceID();
        }

        public void Save()
        {
#if UNITY_EDITOR
            SetAsDirty();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("-------------------Save asset: " + ToString() + " ----------------------------------------------");
#endif
        }

        public void SetAsDirty()
        {
#if UNITY_EDITOR
            if (this)
            {
                EditorUtility.SetDirty(this);
                Debug.Log("-------------------Set dirty: " + ToString() + " ----------------------------------------------");
            }
#endif
        }
    }
}