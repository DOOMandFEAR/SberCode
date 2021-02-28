using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace ARVRLab.UnityDH_Plugin.Server
{
    public partial class ServerAPI
    {
        public static string[] AvaliableVoiceID
        {
            get
            {
                return new string[]
                {
                    "default",
                    "Che_HQ",
                    "Erm_HQ",
                    "She_HQ"
                };
            }
        }

        public static Request<AttributesTTS> GenerateTTSRequest(ParamsTTS paramsTTS)
        {
            var request = new Request<AttributesTTS>
            {
                data = new Data<AttributesTTS>
                {
                    id = Guid.NewGuid().ToString(),
                    type = "textToSpeech",
                    attributes = new AttributesTTS()
                    {
                        tts = paramsTTS
                    }
                }
            };

            return request;
        }

        public static WavFile GenerateTTS(string serverUrl, string serverToken, ParamsTTS tts)
        {
            WavFile result = null;
            var coroutine = GenerateTTSAsync(serverUrl, serverToken, tts, (wav) => result = wav);
            ExecuteRoutineSync(coroutine);

            return result;
        }

        public static IEnumerator GenerateTTSAsync(string serverUrl, string serverToken, ParamsTTS tts, Action<WavFile> onComplete)
        {
            var request = GenerateTTSRequest(tts);
            var json = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            Uri baseUri = new Uri(serverUrl);
            Uri reqUri = new Uri(baseUri, "/dh/api/v1/tts");

            var webRequest = UnityWebRequest.Put(reqUri, json);
            webRequest.method = "POST";
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + serverToken);

            var webRequestHandler = webRequest.SendWebRequest();
            yield return webRequestHandler;

            if (HasError(webRequest, "TTS"))
            {
                onComplete?.Invoke(null);
                yield break;
            }

            var data = webRequest.downloadHandler.data;
            var wavFile = new WavFile(data);
            onComplete?.Invoke(wavFile);
        }
    }
}