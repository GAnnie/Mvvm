using UnityEngine;
using System.Collections;

namespace Foundation.Core.Databinding
{
	[RequireComponent(typeof(UISprite))]
    [AddComponentMenu("Foundation/Databinding/NGUISpriteBinder")]
    public class UISpriteBinder : UIBasicSpriteBinder
	{
		[HideInInspector]
		public BindingInfo SpriteNameBinding = new BindingInfo { BindingName = "SpriteName" };

		public override void Init ()
		{
			base.Init ();
			
			if(Target is UISprite)
			{
				SpriteNameBinding.Action = UpdateSprite;
				SpriteNameBinding.Filters = BindingFilter.Properties;
				SpriteNameBinding.FilterTypes = new[] { typeof(string) };
			}
			else
				Debug.LogWarning("Target is not a UISprite");
		}

		private void UpdateSprite (object arg)
		{
			if (Target) {
				((UISprite)Target).spriteName = (string)arg;
			}
		}
	}
}