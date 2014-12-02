// --------------------------------------
//  Unity Foundation
//  AudioRegulator.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

using System;
using UnityEngine;

namespace Foundation.Core
{
    /// <summary>
    /// audio layers used by the Audio Regulator
    /// </summary>
    public enum AudioLayer
    {
        Sfx,
        Music,
        UISfx,
        None
    }

    /// <summary>
    /// AudioRegulator Attaches onto a conventional AudioSource. 
    /// This script will temper / control the volume based on a static  floating (0.0-1.0f) value. 
    /// The AudioRegulator supports 4 audio layers : SFX, Music, UI, and None.
    /// </summary>
    [AddComponentMenu("Foundation/Audio/AudioRegulator")]
    [RequireComponent(typeof(AudioSource))]
    public class AudioRegulator : MonoBehaviour
    {
        #region static

        /// <summary>
        /// Static settings
        /// </summary>
        public static float SfxVolume = .5f;
        public static float MusicVolume = .5f;
        public static float UISfxVolume = 1f;
        public static float NoneVolume = 1f;

        /// <summary>
        /// Event so regulators know to update
        /// </summary>
        public static event Action OnVolumeChanged;

        /// <summary>
        /// Resets *ALL* Audio regulators
        /// </summary>
        public static void RaiseVolumeChanged()
        {
            if (OnVolumeChanged != null)
                OnVolumeChanged();
        }

        /// <summary>
        /// get volume multiple for layer
        /// </summary>
        /// <returns></returns>
        public static float GetVolume(AudioLayer layer)
        {
            switch (layer)
            {

                case AudioLayer.Sfx:
                    return SfxVolume;

                case AudioLayer.Music:
                    return MusicVolume;

                case AudioLayer.UISfx:
                    return UISfxVolume;
            }

            return NoneVolume;
        }

        #endregion

        /// <summary>
        /// layer this is a member of
        /// </summary>
        public AudioLayer Layer;

        /// <summary>
        /// Read on awake. Multiplied against layer volume
        /// </summary>
        public float LocalVolume { get; set; }

        /// <summary>
        /// Audio source
        /// </summary>
        [HideInInspector]
        public AudioSource Audio;

        protected void Awake()
        {
            Audio = GetComponent<AudioSource>();

            LocalVolume = Audio.volume;

            VolumeChanged();

            OnVolumeChanged += VolumeChanged;

        }

        protected void OnDestroy()
        {
            OnVolumeChanged -= VolumeChanged;
        }

        public void VolumeChanged()
        {
            Audio.volume = LocalVolume * GetVolume(Layer);
        }

        public bool IsPlaying
        {
            get { return Audio.isPlaying; }
        }

        public bool Loop
        {
            get { return Audio.loop; }
            set { Audio.loop = value; }
        }

        public AudioClip Clip
        {
            get { return Audio.clip; }
            set { Audio.clip = value; }
        }

        public void Play()
        {
            audio.Play();
        }
    }
}