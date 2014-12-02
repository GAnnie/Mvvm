#if UNITY_4_6
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Foundation.Core.uGUI
{
    [AddComponentMenu("Foundation/uGUI/SelectOnEnable")]
    public class SelectOnEnable : MonoBehaviour
    {
        public bool DisableOnMobile;

        void OnEnable()
        {
            if (DisableOnMobile && Application.isMobilePlatform)
                return;

            StartCoroutine(OnEnableAsync());
        }

        IEnumerator OnEnableAsync()
        {
            yield return 1;
            var es = EventSystem.current;


            //if it's an input field, also set the text caret
            var inputfield = gameObject.GetComponent<InputField>();
            if (inputfield != null)
                inputfield.OnPointerClick(new PointerEventData(es));

            es.SetSelectedGameObject(gameObject);
        }
    }
}
#endif