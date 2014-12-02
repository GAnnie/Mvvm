using UnityEngine;
using System.Collections;

namespace Foundation.Core.Databinding
{
	[RequireComponent(typeof(UIInput))]
    [AddComponentMenu("Foundation/Databinding/NGUIInputBinder")]
	public class UIInputBinder : BindingBase
	{
		protected UIInput Target;
		[HideInInspector]
		public BindingInfo TextBinding = new BindingInfo { BindingName = "Text" };
		[HideInInspector]
		public BindingInfo SubmitBinding = new BindingInfo { BindingName = "Submit" };
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

			Target = GetComponent<UIInput> ();
			EventDelegate.Add(Target.onSubmit,SubmitText);
			EventDelegate.Add(Target.onChange,ChangeText);

			TextBinding.Action = UpdateText;
			TextBinding.Filters = BindingFilter.Properties;
			TextBinding.FilterTypes = new[] { typeof(string) };

			SubmitBinding.Filters = BindingFilter.Commands;

			EnabledBinding.Action = UpdateState;
			EnabledBinding.Filters = BindingFilter.Properties;
			EnabledBinding.FilterTypes = new[] { typeof(bool) };
		}

		private void SubmitText ()
		{
			SetValue (SubmitBinding.MemberName, null);
		}

		private void UpdateState (object arg)
		{
			Target.enabled = (bool)arg;
		}
      
		private void ChangeText ()
		{
			SetValue (TextBinding.MemberName,Target.value);
		}

		private void UpdateText (object arg)
		{
			if (Target) {
				if (arg!= null && Target.value != arg.ToString())
                {
                    Target.value = arg.ToString();
                }
                else
                {
                    Target.value = string.Empty;
                }
			}
		}
	}
}

