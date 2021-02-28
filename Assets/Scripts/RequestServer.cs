using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARVRLab.UnityDH_Plugin.Server
{
    public class RequestServer : MonoBehaviour
    {
        public UniversalPlayer player;
        public JsonPlayer character;
        public float interpolation = 0.5f;
        
        void Start()
        {
            // SaySomething("Заблудился долговец и кричит. Чего орёшь? Ну я вот услышал и чего теперь делать будем?");

        }

        public void SaySomething(string something)
        {
            ParamsChat chat = null;
            ParamsTTS tts = new ParamsTTS();
            tts.text = something;
            
            ParamsATL atl = new ParamsATL();
            EyeMovementParams eye = new EyeMovementParams();
            eye.eyeMovementMode = "mocap";
            atl.eyeMovementParameters = eye;
            NeckMovementParams neck = new NeckMovementParams();
            neck.neckMovementMode = "mocap";
            atl.neckMovementParameters = neck;


            ComboRequestReply reply = ServerAPI.GenerateCombo("https://sbercode.dh.arvr.sberlabs.com", "PWEgTNRdMJd9s7Qu", chat, tts, atl);

            player.source.clip = reply.wav.GenerateClip();
            character.SetJsonToPlay(reply.animation, player, interpolation);
        }

    }
}