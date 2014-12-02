#if UNITY_4_6
using UnityEngine;
using UnityEngine.UI;

namespace Foundation.Core.Databinding
{
    [RequireComponent(typeof(Text))]
    [AddComponentMenu("Foundation/Databinding/TextBinder")]
    public class TextBinder : BindingBase
    {
        protected Text Label;

        [HideInInspector]
        public BindingInfo LabelBinding = new BindingInfo { BindingName = "Label" };
        [HideInInspector]
        public BindingInfo ColorBinding = new BindingInfo { BindingName = "Color" };

        public string FormatString = string.Empty;


        protected bool IsInit;

        protected void Awake()
        {
            Init();
        }

        public override void Init()
        {
            if (IsInit)
                return;
            IsInit = true;

            Label = GetComponentInChildren<Text>();

            LabelBinding.Action = UpdateLabel;
            LabelBinding.Filters = BindingFilter.Properties;

            ColorBinding.Action = UpdateColor;
            ColorBinding.Filters = BindingFilter.Properties;
            ColorBinding.FilterTypes = new[] { typeof(Color) };
        }


        private void UpdateLabel(object arg)
        {

            var s = arg == null ? string.Empty : arg.ToString();

            if (Label)
            {
                if (string.IsNullOrEmpty(FormatString))
                {
                    Label.text = s;
                }
                else
                {
                    Label.text = string.Format(FormatString, s);
                }
            }
        }

        private void UpdateColor(object arg)
        {
            if (Label)
            {
                Label.color = ((Color)arg);
            }
        }
    }
}
#endif