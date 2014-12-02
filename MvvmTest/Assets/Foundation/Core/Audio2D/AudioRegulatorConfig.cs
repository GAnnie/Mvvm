// --------------------------------------
//  Unity Foundation
//  AudioRegulatorConfig.cs
//  copyright (c) 2014 Nicholas Ventimiglia, http://avariceonline.com
//  All rights reserved.
//  -------------------------------------
// 

using UnityEngine;

namespace Foundation.Core
{
    /// <summary>
    /// AudioRegulatorConfig is a utility script that sets the volume level 
    /// of the AudioRegulator. This is useful for testing or when you 
    /// don’t want a sophisticated menu system to set layer's audio volume.
    /// </summary>
    [AddComponentMenu("Foundation/Audio/AudioRegulatorConfig")]
    public class AudioRegulatorConfig : MonoBehaviour
    {
        public float SfxVolume = .5f;

        public float MusicVolume = .5f;

        public float UISfxVolume = 1f;
    
        void Awake()
        {
            AudioRegulator.SfxVolume = SfxVolume;
            AudioRegulator.MusicVolume = MusicVolume;
            AudioRegulator.UISfxVolume = UISfxVolume;
        }
    }
}