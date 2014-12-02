#if UNITY_4_6
using UnityEngine;
using UnityEngine.UI;

namespace Foundation.Core.Databinding
{
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("Foundation/Databinding/ImageBinder")]
    public class ImageBinder : BindingBase
    {
        protected Image Target;

        [HideInInspector]
        public BindingInfo SpriteBinding = new BindingInfo { BindingName = "Sprite" };
        [HideInInspector]
        public BindingInfo ColorBinding = new BindingInfo { BindingName = "Color" };

        protected bool IsInit;
        protected Sprite original;

        protected void Awake()
        {
            Init();
        }

        public override void Init()
        {
            if (IsInit)
                return;
            IsInit = true;

            Target = GetComponent<Image>();

            original = Target.sprite;

            SpriteBinding.Action = UpdateLabel;
            SpriteBinding.Filters = BindingFilter.Properties;
            SpriteBinding.FilterTypes = new[] { typeof(Texture2D) };


            ColorBinding.Action = UpdateColor;
            ColorBinding.Filters = BindingFilter.Properties;
            ColorBinding.FilterTypes = new[] { typeof(Color) };
        }


        private void UpdateLabel(object arg)
        {
            if (Target)
            {
                var texture = (Texture2D)arg;

                if (texture == null)
                {
                    Target.sprite = original;
                }
                else
                {
                    Target.sprite = Sprite.Create(texture, Target.sprite.rect, Vector2.zero);
                }
            }
        }

        private void UpdateColor(object arg)
        {
            if (Target)
            {
                Target.color = ((Color)arg);
            }
        }
    }
}
#endif
