using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ARVRLab.UnityDH_Plugin.Server;

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

        public Transform cameraOrigin;
        public UniversalPlayer player;
        public JsonPlayer character;
        public Phrase[] phrases;
        public float interpolation = 0.5f;

        public AudioClip startAudio;
        public AudioClip ttsAnswer;
        public TextAsset ttsAnim;
        public AudioClip emptyAudio;
        public TextAsset emptyAnim;

        private int phraseIndex = 0;
        private bool speaking = false;
        private float untillNextPhrase = 0;

        private float untilStart = 10;
        private bool started = false;
        private bool startSpoken = false;
        private bool ttsUsed = false;

        private void Update()
        {
            if (untilStart > 0)
                untilStart -= Time.deltaTime;

            if (untilStart <= 0 && !started)
            {
                started = true;
                AudioSource.PlayClipAtPoint(startAudio, cameraOrigin.position);
                untillNextPhrase = startAudio.length;
            }

            if (started && untillNextPhrase <= 0 && !ttsUsed)
            {
                SaySomething("Understandable, have a great day!");
                ttsUsed = true;
                untillNextPhrase = player.source.clip.length;
            }

            if (ttsUsed && untillNextPhrase <= 0 && !startSpoken)
            {
                speaking = true;
                startSpoken = true;
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

        // Returns time of audio
        private void SaySomething(string something)
        {
            // ParamsChat chat = null;
            // ParamsTTS tts = new ParamsTTS();
            // tts.text = something;
            
            // ParamsATL atl = new ParamsATL();
            // EyeMovementParams eye = new EyeMovementParams();
            // eye.eyeMovementMode = "mocap";
            // atl.eyeMovementParameters = eye;
            // NeckMovementParams neck = new NeckMovementParams();
            // neck.neckMovementMode = "mocap";
            // atl.neckMovementParameters = neck;

            // ComboRequestReply reply = ServerAPI.GenerateCombo("https://sbercode.dh.arvr.sberlabs.com", "PWEgTNRdMJd9s7Qu", chat, tts, atl);

            player.source.clip = ttsAnswer;
            character.SetJsonToPlay(ttsAnim.text, player, interpolation);
        }


    }
}
