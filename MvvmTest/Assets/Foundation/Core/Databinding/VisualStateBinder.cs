using System;
using System.Linq;
using UnityEngine;

namespace Foundation.Core.Databinding
{
    [AddComponentMenu("Foundation/Databinding/VisualStateBinder")]
    public class VisualStateBinder : BindingBase
    {

        public GameObject[] Targets;

        public string ValidState;

        [HideInInspector]
        public BindingInfo ValueBinding = new BindingInfo { BindingName = "State" };

        protected bool IsInit;


        void Awake()
        {
            Init();
        }

        private void UpdateState(object arg)
        {
            var valid = arg != null && ValidState.ToLower() == arg.ToString().ToLower();

            foreach (var target in Targets.ToArray())
            {
                target.SetActive(valid);
            }
        }


        public override void Init()
        {
            if (IsInit)
                return;
            IsInit = true;

            ValueBinding.Action = UpdateState;
            ValueBinding.Filters = BindingFilter.Properties;
            ValueBinding.FilterTypes = new[] { typeof(bool), typeof(string), typeof(int), typeof(Enum) };
        }
    }
}
