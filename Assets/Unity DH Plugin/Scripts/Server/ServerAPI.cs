using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace ARVRLab.UnityDH_Plugin.Server
{
    public partial class ServerAPI
    {
        public static string[] AvaliableEnginesATL
        {
            get
            {
                return new string[]
                {
                    "latest",
                    "elenasb_nvpattexp1-5955-52b68123",
                    "sdfanet_6192_lr01_wd00001_b64_e60_l1_lapa1_lapb1_ds1",
                    "sdfanetb_6199_lr02_wd00001_b64_e60_lh_lapa1_lapb1_naug",
                    "sdfanetd_4525_lr02_wd00001_b128_e120_l1_lapa1_lapb25_naug_ds8_bsw_ea6bd9c6",
                    "sdfanetb39ru_4369_lr02_wd00001_b64_e60_l1_lapa1_lapb2_naug_ds1_bsw_2ed8ff6c"
                };
            }
        }

        public static string[] AvaliableEmotionsATL
        {
            get
            {
                return new string[]
                {
                    "angry",
                    "calm",
                    "curious",
                    "disgust",
                    "doubt",
                    "dreamy",
                    "embarrassment",
                    "fear",
                    "flirt",
                    "happy",
                    "playful",
                    "positive",
                    "resentment",
                    "shame",
                    "sorrow",
                    "strict",
                    "supplication",
                    "surprise",
                    "wrath"
                };
            }
        }

        public static string[] EyesMovementModes
        {
            get
            {
                return new string[]
                {
                    "none",
                    "mocap"
                };
            }
        }

        public static string[] NeckMovementModes
        {
            get
            {
                return new string[]
                {
                    "none",
                    "mocap"
                };
            }
        }

        private static bool HasError(UnityWebRequest webRequest, string requestName)
        {
            if (webRequest.isNetworkError)
            {
                Debug.LogError($"{requestName} failed with network error: " + webRequest.error);
                return true;
            }

            if (webRequest.isHttpError)
            {
                var text = webRequest.downloadHandler.text;
                var errCode = webRequest.responseCode;

                Debug.LogError($"{requestName} failed with response code {errCode}. " +
                    $"Server returned message: {text}");
                return true;
            }

            return false;
        }

        private static void ExecuteRoutineSync(IEnumerator coroutine)
        {
            // execute all coroutine blocks
            while (coroutine.MoveNext())
            {
                var block = coroutine.Current as AsyncOperation;
                while (!block.isDone) ;
            }
        }

    }
}
