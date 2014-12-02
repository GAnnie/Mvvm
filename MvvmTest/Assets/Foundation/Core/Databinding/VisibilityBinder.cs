using System.Linq;
using UnityEngine;

namespace Foundation.Core.Databinding
{
    [AddComponentMenu("Foundation/Databinding/VisibilityBinder")]
    public class VisibilityBinder : BindingBase
    {

        public GameObject[] Targets;
        public GameObject[] InverseTargets;

        [HideInInspector]
        public BindingInfo ValueBinding = new BindingInfo { BindingName = "Value" };

        protected bool IsInit;


        void Awake()
        {
            Init();
        }

        private void UpdateState(object arg)
        {
            var b = arg != null && (bool)arg;

            foreach (var target in Targets)
            {
                if (target)
                    target.SetActive(b);
            }

            foreach (var target in InverseTargets)
            {
                if (target)
                    target.SetActive(!b);
            }
        }


        public override void Init()
        {
            if (IsInit)
                return;
            IsInit = true;

            ValueBinding.Action = UpdateState;
            ValueBinding.Filters = BindingFilter.Properties;
            ValueBinding.FilterTypes = new[] { typeof(bool) };
        }
    }
}
