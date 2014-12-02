#if UNITY_4_6
using UnityEngine;
using UnityEngine.UI;

namespace Foundation.Core.Databinding
{
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("Foundation/Databinding/TweenColorBinder")]
    public class TweenColorBinder : BindingBase
    {
        [HideInInspector]
        public BindingInfo ValueBinding = new BindingInfo { BindingName = "Value" };

        public Color MinColor = Color.red;

        public Color MaxColor = Color.green;

        public Image image;

        protected bool IsInit;

        protected void Awake()
        {
            image = GetComponent<Image>();

            Init();
        }

        public override void Init()
        {
            if (IsInit)
                return;
            IsInit = true;

            ValueBinding.Action = UpdateColor;
            ValueBinding.Filters = BindingFilter.Properties;
            ValueBinding.FilterTypes = new[] { typeof(float) };
        }


        private void UpdateColor(object arg)
        {
            var f = (float)arg;

            image.color = Color.Lerp(MinColor, MaxColor, f);
        }
    }
}
#endif