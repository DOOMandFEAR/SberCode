using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARVRLab.UnityDH_Plugin.Examples
{
    public class PhraseSwitcher : MonoBehaviour
    {
        [System.Serializable]
        public struct Phrase
        {
            public TextAsset anim;
            public AudioClip audio;
        }

        public UniversalPlayer player;
        public JsonPlayer character;
        public Phrase[] phrases;
        public float interpolation = 0.5f;

        public AudioClip emptyAudio;
        public TextAsset emptyAnim;

        private int phraseIndex = 0;
        private bool speaking = false;
        private float untillNextPhrase = 0;

        private float untilStart = 10;
        private bool started = false;

        private void Update()
        {
            if (untilStart > 0)
                untilStart -= Time.deltaTime;

            if (untilStart <= 0 && !started)
            {
                speaking = true;
                started = true;
            }

            if (speaking)
            {
                if (untillNextPhrase <= 0)
                {
                    if (phraseIndex >= phrases.Length)
                        StopSpeaking();
                    else
                    {
                        var phrase = phrases[phraseIndex];
                        player.source.clip = phrase.audio;
                        character.SetJsonToPlay(phrase.anim.text, player, interpolation);

                        untillNextPhrase = phrase.audio.length;

                        phraseIndex++;
                    }
                }
                
            }

            if (untillNextPhrase > 0)
                untillNextPhrase -= Time.deltaTime;
        }

        private void StopSpeaking()
        {
            phraseIndex = 0;
            untillNextPhrase = 0;
            player.source.clip = emptyAudio;
            character.SetJsonToPlay(emptyAnim.text, player, interpolation);
            speaking = false;
        }


    }
}
