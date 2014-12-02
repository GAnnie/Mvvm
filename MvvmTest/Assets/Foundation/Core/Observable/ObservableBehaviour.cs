// --------------------------------------
//  Unity Foundation
//  BindableBehaviour.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

using System;
using UnityEngine;

namespace Foundation.Core
{


    /// <summary>
    /// Implements the IBindable for mono behavior objects
    /// </summary>
    public abstract class ObservableBehaviour : MonoBehaviour, IObservableModel
    {
        public event Action<ObservableMessage> OnBindingUpdate;

        private ModelBinder _binder;

        private bool _isDisposed;

        private ObservableMessage _bindingMessage;

        private ModelBinder Binder
        {
            get
            {
                if (_binder == null && !_isDisposed)
                    _binder = new ModelBinder(this);
                return _binder;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool IsApplicationQuit { get; private set; }

        protected virtual void Awake()
        {
            _bindingMessage = new ObservableMessage { Sender = this };
            if (_binder == null)
                _binder = new ModelBinder(this);
        }

        protected virtual void OnDestroy()
        {
            if (IsApplicationQuit)
                return;

            Dispose();
        }

        [HideInInspector]
        public virtual void Dispose()
        {
            _isDisposed = true;

            if (_binder != null)
            {
                _binder.Dispose();
            }

            if (_bindingMessage != null)
            {
                _bindingMessage.Dispose();
            }

            _bindingMessage = null;
            _binder = null;
        }

        public void RaiseBindingUpdate(string memberName, object paramater)
        {
            Binder.RaiseBindingUpdate(memberName, paramater);

            if (OnBindingUpdate != null)
            {
                _bindingMessage.Name = memberName;
                _bindingMessage.Value = paramater;
                OnBindingUpdate(_bindingMessage);
            }
        }


        [HideInInspector]
        public void Command(string memberName)
        {
            _binder.Command(memberName);
        }

        public void Command(string memberName, object paramater)
        {
            _binder.Command(memberName, paramater);
        }

        [HideInInspector]
        public object GetValue(string memberName)
        {
            return Binder.GetValue(memberName);
        }

        public object GetValue(string memberName, object paramater)
        {

            return Binder.GetValue(memberName, paramater);
        }

        public void NotifyProperty(string memberName, object paramater)
        {
            RaiseBindingUpdate(memberName, paramater);
        }

        protected virtual void OnApplicationQuit()
        {
            IsApplicationQuit = true;
        }
    }
}