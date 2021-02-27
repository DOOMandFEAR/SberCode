using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace ARVRLab.UnityDH_Plugin.Server
{
    public partial class ServerAPI
    {
        public static string GenerateLipSync(string serverUrl, string serverToken, ParamsATL atl, byte[] wavFile)
        {
            string result = null;
            var coroutine = GenerateLipSyncAsync(serverUrl, serverToken, atl, wavFile, (json) => result = json);
            ExecuteRoutineSync(coroutine);

            return result;
        }

        public static IEnumerator GenerateLipSyncAsync(string serverUrl, string serverToken, ParamsATL atl, byte[] wavFile, Action<string> onComplete)
        {
            var request = GenerateATLRequest(atl);
            var json = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            Uri baseUri = new Uri(serverUrl);
            Uri reqUri = new Uri(baseUri, "/dh/api/v1/atl");

            var form = new WWWForm();
            form.AddBinaryData("audio", wavFile);
            form.AddField("json", json);

            var webRequest = UnityWebRequest.Post(reqUri, form);
            webRequest.SetRequestHeader("Authorization", "Bearer " + serverToken);

            var webRequestHandler = webRequest.SendWebRequest();
            yield return webRequestHandler;

            if (HasError(webRequest, "ATL"))
            {
                onComplete?.Invoke(null);
                yield break;
            }

            var data = webRequest.downloadHandler.text;
            onComplete?.Invoke(data);
        }

        public static Request<AttributesATL> GenerateATLRequest(ParamsATL atl)
        {
            var request = new Request<AttributesATL>
            {
                data = new Data<AttributesATL>
                {
                    id = Guid.NewGuid().ToString(),
                    type = "audioToLipSync",
                    attributes = new AttributesATL()
                    {
                        atl = atl
                    }
                }
            };

            return request;
        }
    }
}
