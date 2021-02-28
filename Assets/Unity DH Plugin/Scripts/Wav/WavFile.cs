using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WWUtils.Audio;

namespace ARVRLab.UnityDH_Plugin
{
    public class WavFile
    {
        public byte[] Bytes { get; set;}

        public WavFile(byte[] rawWavBytes)
        {
            Bytes = rawWavBytes;
        }

        public AudioClip GenerateClip()
        {
            try
            {
                var wav = new WAV(Bytes);
                var clip = AudioClip.Create("testSound", wav.SampleCount, 1, wav.Frequency, false);
                clip.SetData(wav.LeftChannel, 0);
                return clip;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return null;
            }
        }
    }
}