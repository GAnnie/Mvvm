﻿#if UNITY_4_6
using UnityEngine;
using UnityEngine.UI;

namespace Foundation.Core.Databinding
{
    [RequireComponent(typeof(Slider))]
    [AddComponentMenu("Foundation/Databinding/SliderBinder")]
    public class SliderBinder : BindingBase
    {
        protected Slider Target;

        [HideInInspector]
        public BindingInfo ValueBinding = new BindingInfo { BindingName = "Value" };

        [HideInInspector]
        public BindingInfo EnabledBinding = new BindingInfo { BindingName = "Enabled" };

        [HideInInspector]
        public BindingInfo MinValue = new BindingInfo { BindingName = "MinValue" };

        [HideInInspector]
        public BindingInfo MaxValue = new BindingInfo { BindingName = "MaxValue" };

        protected bool IsInit;

        public AudioClip SwipeSound;

        public float SoundLag = .5f;

        public bool ReadOnly = false;

        protected float NextSwipe;
        protected void Awake()
        {
            Init();
        }

        public override void Init()
        {
            if (IsInit)
                return;
            IsInit = true;

            NextSwipe = Time.time + SoundLag;

            Target = GetComponent<Slider>();
            if (!ReadOnly)
                Target.onValueChanged.AddListener(HandleChange);

            ValueBinding.Action = UpdateSlider;
            ValueBinding.Filters = BindingFilter.Properties;
            ValueBinding.FilterTypes = new[] { typeof(float) };

            MinValue.Action = UpdateMin;
            MinValue.Filters = BindingFilter.Properties;
            MinValue.FilterTypes = new[] { typeof(float) };

            MaxValue.Action = UpdateMax;
            MaxValue.Filters = BindingFilter.Properties;
            MaxValue.FilterTypes = new[] { typeof(float) };

            EnabledBinding.Action = UpdateEnabled;
            EnabledBinding.Filters = BindingFilter.Properties;
            EnabledBinding.FilterTypes = new[] { typeof(bool) };
        }

        private void UpdateEnabled(object arg)
        {
            Target.interactable = (bool)arg;
        }

        private void HandleChange(float arg)
        {
            if (!Application.isPlaying)
                return;

            if (SwipeSound != null)
            {
                if (NextSwipe < Time.time)
                {
                    Audio2DListener.PlayUI(SwipeSound, 1);
                    NextSwipe = Time.time + SoundLag;
                }
            }

            SetValue(ValueBinding.MemberName, arg);
        }

        private void UpdateSlider(object arg)
        {
            if (Target)
            {
                Target.value = (float)arg;
            }
        }
        private void UpdateMin(object arg)
        {
            if (Target)
            {
                Target.minValue = (float)arg;
            }
        }
        private void UpdateMax(object arg)
        {
            if (Target)
            {
                Target.maxValue = (float)arg;
            }
        }
    }
}
#endif