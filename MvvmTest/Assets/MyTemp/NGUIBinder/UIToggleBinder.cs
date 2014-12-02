using UnityEngine;
using System.Collections;

namespace Foundation.Core.Databinding
{
	[RequireComponent(typeof(UIToggle))]
    [AddComponentMenu("Foundation/Databinding/NGUIToggleBinder")]
    public class UIToggleBinder : BindingBase
	{
		protected UIToggle Target;
		[HideInInspector]
		public BindingInfo EnabledBinding = new BindingInfo
        {
            BindingName = "Enabled"
        };
		[HideInInspector]
		public BindingInfo ValueBinding = new BindingInfo
        {
            BindingName = "Value"
        };
		[HideInInspector]
		public BindingInfo GroupBinding = new BindingInfo
        {
            BindingName = "Group"
        };

		protected bool IsInit;

		void Awake ()
		{
			Init ();
		}
		
		public override void Init ()
		{
			if (IsInit)
				return;
			IsInit = true;
			
			Target = GetComponent<UIToggle> ();
			EventDelegate.Add (Target.onChange, Call);
			
			ValueBinding.Filters = BindingFilter.Properties;
			ValueBinding.FilterTypes = new[] { typeof(bool) };
			ValueBinding.Action = UpdateValue;

			EnabledBinding.Action = UpdateState;
			EnabledBinding.Filters = BindingFilter.Properties;
			EnabledBinding.FilterTypes = new[] { typeof(bool) };
			
			GroupBinding.Action = UpdateGroup;
			GroupBinding.Filters = BindingFilter.Properties;
			GroupBinding.FilterTypes = new [] { typeof(int)};
		}

		private void Call ()
		{
			bool value = UIToggle.current.value;
			SetValue (ValueBinding.MemberName, value);
		}

		private void UpdateValue (object o)
		{
			Target.value = (bool)o;
		}

		private void UpdateState (object arg)
		{
			Target.enabled = (bool)arg;
		}
		
		private void UpdateGroup(object arg)
		{
			Target.group = (int)arg;	
		}
	}
}