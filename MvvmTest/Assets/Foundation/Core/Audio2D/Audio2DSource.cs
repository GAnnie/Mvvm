// --------------------------------------
//  Unity Foundation
//  Audio2DSource.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

using System;
using System.Collections;
using UnityEngine;

namespace Foundation.Core
{
    /// <summary>
    /// Audio2DListener is a control for playing audio clips via  the Audio2DListener. 
    /// This control is analogous to Unit's Audio Source... 
    /// except for the fact that its playing position is consistent.
    /// </summary>
    [Serializable]
    [AddComponentMenu("Foundation/Audio/Audio2DSource")]
    public class Audio2DSource : MonoBehaviour
    {
        // settings
        public AudioClip Clip;
        public float Volume = 1;
        public float Pitch = 1;
        public bool Loop = false;
        public int Delay = 0;
        public AudioLayer Layer = AudioLayer.UISfx;

        // play on enabled
        public bool PlayOnEnable = true;

        // set internally
        public bool IsPlaying { get; set; }

        // temp
        public AudioSource Source { get; set; }

        protected bool FirstTime = true;

        protected Audio2DMessage Message = new Audio2DMessage();


        protected void OnEnable()
        {
            if (PlayOnEnable)
                Play();
        }

        /// <summary>
        /// play audio with delay
        /// </summary>
        public void Play()
        {
            StartCoroutine(PlayAsync(Delay));
        }

        /// <summary>
        /// play audio
        /// </summary>
        /// <param name="delay"></param>
        public void Play(float delay)
        {
            StartCoroutine(PlayAsync(delay));
        }

        IEnumerator PlayAsync(float delay)
        {
            if (Source)
            {
                Source.Stop();
            }

            IsPlaying = true;

            if (delay > 0)
                yield return new WaitForSeconds(delay);

            Message.Loop = Loop;
            Message.Play = true;
            Message.Pitch = Pitch;
            Message.Volume = Volume * AudioRegulator.GetVolume(Layer);
            Message.Clip = Clip;

            Message.Publish();

            Source = Message.Source;

            while (Source != null && Source.isPlaying)
            {
                yield return 1;
            }

            Source = null;
            IsPlaying = false;
        }
    }
}