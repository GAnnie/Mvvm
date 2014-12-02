using UnityEngine;
using System.Collections;

namespace Foundation.Core.Databinding
{
	[RequireComponent(typeof(UIButton))]
    [AddComponentMenu("Foundation/NGUIDatabinding/NGUIButtonBinder")]
	public class UIButtonBinder : BindingBase
	{
		protected UIButton Button;
		protected UIButtonParamater Paramater;
		
		[HideInInspector]
        public BindingInfo EnabledBinding = new BindingInfo { BindingName = "Enabled" };

        [HideInInspector]
        public BindingInfo OnClickBinding = new BindingInfo { BindingName = "OnClick" };
		
		protected bool IsInit;

        void Awake()
        {
            Init();
        }
		
		public override void Init()
        {
            if (IsInit)
                return;
            IsInit = true;

            Paramater = GetComponent<UIButtonParamater>();
            Button = GetComponent<UIButton>();

            OnClickBinding.Filters = BindingFilter.Commands;
            
            EnabledBinding.Action = UpdateState;
            EnabledBinding.Filters = BindingFilter.Properties;
            EnabledBinding.FilterTypes = new[] { typeof(bool) };
            
          	EventDelegate.Add(Button.onClick,Call);
        }

        public void Call()
        {
            SetValue(OnClickBinding.MemberName, Paramater == null? null : Paramater.GetValue());
        }

        private void UpdateState(object arg)
        {
            Button.isEnabled = (bool)arg;
        }
	}
}

