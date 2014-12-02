using UnityEngine;

namespace Foundation.Core.Databinding
{
    [AddComponentMenu("Foundation/Databinding/SpawnInstanceBinder")]
    public class SpawnInstanceBinder : BindingBase
    {

        public Transform Target;

        protected GameObject instance;

        [HideInInspector]
        public BindingInfo ValueBinding = new BindingInfo { BindingName = "Prefab" };

        protected bool IsInit;

        public bool DisplayMode;

        public bool MakeChild;


        void Awake()
        {
            Init();
        }

        private void UpdateState(object arg)
        {

            if (instance != null)
            {
                Destroy(instance);
            }

            if (arg != null)
            {
                if (Target != null)
                {
                    instance = (GameObject)Instantiate((GameObject)arg, Target.position, Target.rotation);

                    if (MakeChild)
                    {
                        instance.transform.parent = Target.transform;
                    }
                }
                else
                {
                    instance = (GameObject)Instantiate((GameObject)arg, Vector3.zero, Quaternion.identity);
                }

                if (DisplayMode)
                    instance.SendMessage("DisplayMode", SendMessageOptions.DontRequireReceiver);

            }
        }


        public override void Init()
        {
            if (IsInit)
                return;
            IsInit = true;

            ValueBinding.Action = UpdateState;
            ValueBinding.Filters = BindingFilter.Properties;
            ValueBinding.FilterTypes = new[] { typeof(GameObject) };
        }
    }
}