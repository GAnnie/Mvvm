﻿#if UNITY_4_6
using UnityEngine;

namespace Foundation.Core.Databinding
{
    /// <summary>
    /// Adds a Paramater to the Button Command
    /// </summary>
    [RequireComponent(typeof(ButtonBinder))]
    [AddComponentMenu("Foundation/Databinding/ButtonParamater")]
    public class ButtonParamater : BindingBase
    {
        public enum ParamaterTypeEnum
        {
            Context,
            Static,
            Binding,

        }

        [HideInInspector]
        public BindingInfo ParameterBinding = new BindingInfo { BindingName = "Parameter" };

        public ParamaterTypeEnum ParamaterType;

        public string StaticParamater;

        protected bool IsInit;

        void Awake()
        {
            Init();
        }

        public override void Init()
        {
            if (IsInit)
                return;

            IsInit = true;

            ParameterBinding.Filters = BindingFilter.Properties;
            ParameterBinding.ShouldShow = HasParamaterBinding;
        }

        public object GetValue()
        {
            switch (ParamaterType)
            {
                case ParamaterTypeEnum.Binding:
                    return GetValue(ParameterBinding.MemberName);
                case ParamaterTypeEnum.Context:
                    return Context.DataInstance;
                case ParamaterTypeEnum.Static:
                    return StaticParamater;
            }

            return null;
        }

        bool HasParamaterBinding()
        {
            return ParamaterType == ParamaterTypeEnum.Binding;
        }


    }
}
#endif