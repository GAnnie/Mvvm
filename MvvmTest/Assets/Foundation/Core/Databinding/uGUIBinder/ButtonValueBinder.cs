#if UNITY_4_6
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Foundation.Core.Databinding
{
        [RequireComponent(typeof(Button))]
    [AddComponentMenu("Foundation/Databinding/ButtonValueBinder")]
    public class ButtonValueBinder : BindingBase, IPointerDownHandler, IPointerUpHandler
    {

        protected Button Button;
        protected ButtonParamater Paramater;

        [HideInInspector]
        public BindingInfo EnabledBinding = new BindingInfo { BindingName = "Enabled" };

        [HideInInspector]
        public BindingInfo IsPressedBinding = new BindingInfo { BindingName = "IsPressed" };

        protected bool IsInit;

        void Awake()
        {
            Init();
        }


        private void UpdateState(object arg)
        {
            Button.interactable = (bool)arg;
        }


        public override void Init()
        {
            if (IsInit)
                return;
            IsInit = true;

            Paramater = GetComponent<ButtonParamater>();
            Button = GetComponent<Button>();

            IsPressedBinding.Filters = BindingFilter.Properties;
            IsPressedBinding.FilterTypes = new[] { typeof(bool) };

            EnabledBinding.Action = UpdateState;
            EnabledBinding.Filters = BindingFilter.Properties;
            EnabledBinding.FilterTypes = new[] { typeof(bool) };
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SetValue(IsPressedBinding.MemberName, true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SetValue(IsPressedBinding.MemberName, false);
        }
    }
}
#endif