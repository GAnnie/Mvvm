using UnityEngine;
using System.Collections;

namespace Foundation.Core.Databinding
{
	[RequireComponent(typeof(UIWidget))]
    [AddComponentMenu("Foundation/Databinding/NGUIWidgetBinder")]
    public class UIWidgetBinder : BindingBase
	{
		protected UIWidget Target;
		[HideInInspector]
		public BindingInfo ColorBinding = new BindingInfo { BindingName = "Color" };
		[HideInInspector]
		public BindingInfo AlphaBinding = new BindingInfo { BindingName = "Alpha" };
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

			Target = GetComponent<UIWidget> ();

			ColorBinding.Action = UpdateColor;
			ColorBinding.Filters = BindingFilter.Properties;
			ColorBinding.FilterTypes = new[] { typeof(Color) };
			
			AlphaBinding.Action = UpdateAlpha;
			AlphaBinding.Filters = BindingFilter.Properties;
			AlphaBinding.FilterTypes = new[] { typeof(float)};
		}
		
		protected void UpdateColor (object arg)
		{
			if (Target) {
				Target.color = ((Color)arg);
			}
		}
		
		protected void UpdateAlpha (object arg)
		{
			if (Target) {
				Target.alpha = ((float)arg);
			}
		}
	}
}