using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Foundation.Core.Editor
{
    public class InstantiateScriptableObjects
    {
        [MenuItem("Foundation/Instantiate ScriptableObjects")]
        public static void ShowWindow()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes().Where(o => o.HasAttribute<InjectorService>())).ToArray();
            
            foreach (var type in types)
            {
                if (!typeof(ScriptableObject).IsAssignableFrom(type))
                    continue;

                var deco = type.GetAttribute<InjectorService>();

                if (deco.AbortLoad)
                    continue;

                // note Object has the responsibility of exporting itself to the injector
                var resource = Resources.Load(deco.ResourceName);

                // already an instance
                if (resource != null)
                    continue;

                var path = Application.dataPath + "/Resources";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var asset = ScriptableObject.CreateInstance(type);
                AssetDatabase.CreateAsset(asset, "Assets/Resources/"+deco.ResourceName+".asset");
                AssetDatabase.SaveAssets();

                Debug.Log(type.Name + " Created");
            }
        }
    }
}