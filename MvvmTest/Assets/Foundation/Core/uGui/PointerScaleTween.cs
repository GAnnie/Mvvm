﻿#if UNITY_4_6
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Foundation.Core.uGUI
{
    /// <summary>
    /// nGui style scale hover
    /// </summary>
    [AddComponentMenu("Foundation/uGUI/PointerScaleTween")]
    public class PointerScaleTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        public float NormalScale = 1f;

        public float HoverScale = 1.1f;

        public float DownScale = 1.05f;

        public float TweenTime = .1f;

        protected RectTransform Transform;
        protected bool IsOver;
        protected bool IsDown;

        private Button Button;

        void Awake()
        {
            Button = GetComponent<Button>();
            Transform = GetComponent<RectTransform>();
        }

        void OnDisable()
        {
            Transform.localScale = Vector3.one * NormalScale;
            IsOver = false;
            IsDown = false;
            StopAllCoroutines();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Button && !Button.IsInteractable())
                return;

            IsOver = true;

            if (!gameObject.activeInHierarchy)
                return;

            StopAllCoroutines();
            StartCoroutine(TweenTo(NormalScale, HoverScale));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (Button && !Button.IsInteractable())
                return;

            IsOver = false;

            if(!gameObject.activeInHierarchy)
                return;

            StopAllCoroutines();
            StartCoroutine(TweenTo(HoverScale, NormalScale));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Button && !Button.IsInteractable())
                return;

            IsDown = true;

            if (!gameObject.activeInHierarchy)
                return;

            StopAllCoroutines();
            StartCoroutine(TweenTo(HoverScale, DownScale));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (Button && !Button.IsInteractable())
                return;

            IsDown = false;

            if (!gameObject.activeInHierarchy)
                return;

            StopAllCoroutines();
            if (IsOver)
                StartCoroutine(TweenTo(DownScale, HoverScale));
        }

        IEnumerator TweenTo(float from, float to)
        {
            float delta = 0;
            do
            {
                yield return 1;
                delta += Time.deltaTime;

                Transform.localScale = Vector3.one * Mathf.Lerp(from, to, delta / TweenTime);

            } while (delta < TweenTime);

            yield return 1;

            Transform.localScale = Vector3.one * to;
        }

    }
}
#endif