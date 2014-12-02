using System;
using UnityEngine;

namespace Foundation.Core
{
    /// <summary>
    /// Sent via messenger.
    /// Instructs the playing of 2audio
    /// </summary>
    [Serializable]
    public class Audio2DMessage : IMessengerObject
    {
        /// <summary>
        /// Clip to play
        /// </summary>
        public AudioClip Clip;

        /// <summary>
        /// Volume
        /// </summary>
        public float Volume = 1;

        /// <summary>
        /// Should Loop
        /// </summary>
        public float Pitch = 1;

        /// <summary>
        /// Should Loop
        /// </summary>
        public bool Loop;

        /// <summary>
        /// should play
        /// </summary>
        public bool Play = true;

        /// <summary>
        /// assigned by service (for loop canceling)
        /// </summary>
        public AudioSource Source { get; set; }
    }
}