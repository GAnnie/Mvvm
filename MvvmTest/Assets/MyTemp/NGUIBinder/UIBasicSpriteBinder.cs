using UnityEngine;
using System.Collections;

namespace Foundation.Core.Databinding
{
	public abstract class UIBasicSpriteBinder : UIWidgetBinder
	{
		[HideInInspector]
		public BindingInfo FillValueBinding = new BindingInfo { BindingName = "FillValue" };

		public override void Init ()
		{
			base.Init ();

			FillValueBinding.Action = UpdateFill;
			FillValueBinding.Filters = BindingFilter.Properties;
			FillValueBinding.FilterTypes = new[] { typeof(float) };
			FillValueBinding.ShouldShow = () => {
				return ((UIBasicSprite)Target).type == UIBasicSprite.Type.Filled;};
		}
		
		protected void UpdateFill (object arg)
		{
			if (Target) {
				((UIBasicSprite)Target).fillAmount = (float)arg;
			}
		}
	}
}