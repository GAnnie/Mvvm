using UnityEngine;
using System.Collections;

namespace Foundation.Core.Databinding
{
	[RequireComponent(typeof(UITexture))]
	[AddComponentMenu("Foundation/Databinding/NGUITextureBinder")]
	public class UITextureBinder : UIBasicSpriteBinder
	{
		[HideInInspector]
		public BindingInfo TextureBinding = new BindingInfo { BindingName = "Texture" };

		public override void Init ()
		{
			base.Init ();
			
			if(Target is UITexture)
			{
				TextureBinding.Action = UpdateTexture;
				TextureBinding.Filters = BindingFilter.Properties;
				TextureBinding.FilterTypes = new[] { typeof(Texture) };
			}
			else
				Debug.LogWarning("Target is not a UITexture");
		}

		private void UpdateTexture (object arg)
		{
			if (Target) {
				var texture = (Texture)arg;

				Target.mainTexture = texture;
			}
		}
	}
}

