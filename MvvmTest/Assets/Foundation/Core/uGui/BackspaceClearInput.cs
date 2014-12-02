#if UNITY_4_6
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Foundation.Core.uGUI
{
    [RequireComponent(typeof(InputField))]
    [AddComponentMenu("Foundation/uGUI/BackspaceClearInput")]
    public class BackspaceClearInput : MonoBehaviour
    {
        protected InputField field;
        protected EventSystem system;

        protected void Awake()
        {
            field = GetComponent<InputField>();
            system = EventSystem.current;
        }


        // BUG
        //void LateUpdate()
        //{

        //    if (system.currentSelectedObject == gameObject)
        //    {
        //        if (Input.GetKeyUp(KeyCode.Backspace))
        //        {
        //            field.value = string.Empty;
        //        }
        //    }
        //}
    }
}
#endif