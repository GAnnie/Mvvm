using UnityEngine;

namespace Foundation.Core
{
    /// <summary>
    /// exit app command
    /// </summary>
    [AddComponentMenu("Foundation/Terminal/Quit")]
    public class QuitCommand : MonoBehaviour
    {
        void Awake()
        {


#if UNITY_EDITOR
             Terminal.Add(new TerminalCommand
            {
                Label = "Quit",
                Method = () =>
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                }
            });
#elif UNITY_STANDALONE || UNITY_ANDROID
             Terminal.Add(new TerminalCommand
            {
                Label = "Quit",
                Method = () => Application.Quit()
            });
#endif

        }
    }
}