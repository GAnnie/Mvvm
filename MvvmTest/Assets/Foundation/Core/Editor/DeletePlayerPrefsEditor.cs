using UnityEditor;
using UnityEngine;

namespace Foundation.Core.Editor
{
    public class DeletePlayerPrefsEditor
    {
        [MenuItem("Foundation/Delete All Player Prefs")]
        public static void CreateAppServices()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}