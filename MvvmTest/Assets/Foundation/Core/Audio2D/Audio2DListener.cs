// --------------------------------------
//  Unity Foundation
//  Audio2DListener.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Foundation.Core
{
    /// <summary>
    /// Audio2DListener attaches onto the MainCamera's Audio Listener.
    ///  This script acts as a central point to playing 2d (UI) audio. 
    /// This script spawns audio sources that play as needed. 
    /// Playing audio is handled via the Audio2DMessage.
    /// 
    /// This is needed because during game-play the MainCamera moves.
    /// </summary>
    [Serializable]
    [AddComponentMenu("Foundation/Audio/Audio2dListener")]
    [RequireComponent(typeof(AudioListener))]
    public class Audio2DListener : MonoBehaviour
    {
        protected static List<AudioSource> Children = new List<AudioSource>();

        protected static Transform Transform;

        protected void Awake()
        {
            Transform = transform;
            Messenger.Subscribe(this);
        }

        protected void OnDestroy()
        {
            Transform = null;
            Messenger.Unsubscribe(this);
            Children.Clear();
        }

        [Subscribe]
        public void OnMessage(Audio2DMessage message)
        {
            var source = GetNext();

            source.pitch = message.Pitch;
            source.loop = message.Loop;
            source.volume = message.Volume;
            source.clip = message.Clip;

            message.Source = source;

            if (message.Play)
                message.Source.Play();
        }

        /// <summary>
        /// will return null if no channels available
        /// </summary>
        /// <returns></returns>
        public static AudioSource GetNext()
        {
            for (int i = 0; i < Children.Count; i++)
            {
                if (Children[i] == null)
                {
                    // this happens on level changing.
                    return null;
                }

                if (Children[i].isPlaying)
                    continue;

                return Children[i];
            }

            var go = new GameObject("_Audio2d");
            go.transform.parent = Transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            var s = go.AddComponent<AudioSource>();

            Children.Add(s);

            return s;
        }

        public static AudioSource PlayUI(AudioClip c, float v)
        {
            return Play(c, v, 1, AudioLayer.UISfx);
        }

        public static AudioSource PlayUI(AudioClip c, float v, float p)
        {
            return Play(c, v, p, AudioLayer.UISfx);
        }


        public static AudioSource Play(AudioClip c, float v)
        {
            return Play(c, v, 1, AudioLayer.None);
        }

        public static AudioSource Play(AudioClip c, float v, float p)
        {
            return Play(c, v, p, AudioLayer.None);
        }

        public static AudioSource Play(AudioClip c, float v, float p, AudioLayer l)
        {
            var s = GetNext();

            // return null
            if (s == null)
                return null;

            s.clip = c;
            s.pitch = p;
            s.loop = false;
            s.volume = v * AudioRegulator.GetVolume(l);
            s.Play();
            return s;
        }
    }
}