using UnityEngine;
using System.Collections;

namespace Foundation.Core.Databinding
{
	[RequireComponent(typeof(UISlider))]
    [AddComponentMenu("Foundation/Databinding/NGUISliderBinder")]
    public class UISliderBinder : BindingBase
	{
		protected UISlider Target;
		[HideInInspector]
		public BindingInfo ValueBinding = new BindingInfo { BindingName = "Value" };
		[HideInInspector]
		public BindingInfo EnabledBinding = new BindingInfo { BindingName = "Enabled" };
		protected bool IsInit;

		protected void Awake ()
		{
			Init ();
		}

		public override void Init ()
		{
			if (IsInit)
				return;
			IsInit = true;

			Target = GetComponent<UISlider> ();
			EventDelegate.Add(Target.onChange,HandleChange);

			ValueBinding.Action = UpdateSlider;
			ValueBinding.Filters = BindingFilter.Properties;
			ValueBinding.FilterTypes = new[] { typeof(float) };

			EnabledBinding.Action = UpdateEnabled;
			EnabledBinding.Filters = BindingFilter.Properties;
			EnabledBinding.FilterTypes = new[] { typeof(bool) };
		}

		private void UpdateEnabled (object arg)
		{
			Target.enabled = (bool)arg;
		}

		private void HandleChange ()
		{
			if (!Application.isPlaying)
				return;

			SetValue (ValueBinding.MemberName, UISlider.current.value);
		}

		private void UpdateSlider (object arg)
		{
			if (Target) {
				Target.value = (float)arg;
			}
		}
	}
}