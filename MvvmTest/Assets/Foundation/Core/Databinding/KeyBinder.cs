using UnityEngine;

namespace Foundation.Core.Databinding
{
    [AddComponentMenu("Foundation/Databinding/KeyBinder")]
    public class KeyBinder : BindingBase
    {
        [HideInInspector]
        public BindingInfo CommandBinding = new BindingInfo { BindingName = "Command" };

        public KeyCode Key;

        public bool RequireDouble = false;
        protected float lastHit;

        public AudioClip ClickSound;

        void Awake()
        {
            Init();
        }

        public void Call()
        {
            if (ClickSound)
            {
                Audio2DListener.PlayUI(ClickSound, 1);
            }

            SetValue(CommandBinding.MemberName, null);
        }
        
      
        public override void Init()
        {
            CommandBinding.Filters = BindingFilter.Commands;
        }


        void Update()
        {
            if (Input.GetKeyUp(Key))
            {
                if (RequireDouble)
                {
                    if (lastHit + .2f > Time.time)
                    {
                        Call();
                        lastHit = 0;
                    }
                    lastHit = Time.time;
                }
                else
                {
                    Call();
                }
            }
        }
    }
}
