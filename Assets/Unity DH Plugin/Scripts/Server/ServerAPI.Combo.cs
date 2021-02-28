using Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using UnityEngine.Networking;

namespace ARVRLab.UnityDH_Plugin.Server
{
    public class ComboRequestReply
    {
        public WavFile wav;
        public string animation;
    }

    public partial class ServerAPI
    {
        public static Request<AttributesCombo> GenerateComboRequest(ParamsChat chat, ParamsTTS tts, ParamsATL atl)
        {
            var request = new Request<AttributesCombo>
            {
                data = new Data<AttributesCombo>
                {
                    id = Guid.NewGuid().ToString(),
                    type = "textToSpeech",
                    attributes = new AttributesCombo()
                    {
                        chat = chat,
                        tts = tts,
                        atl = atl
                    }
                }
            };

            return request;
        }

        public static ComboRequestReply GenerateCombo(string serverUrl, string dhsToken,
            ParamsChat chat, ParamsTTS tts, ParamsATL atl)
        {
            ComboRequestReply result = null;
            var coroutine = GenerateComboAsync(serverUrl, dhsToken, chat, tts, atl, (ans) => result = ans);
            ExecuteRoutineSync(coroutine);

            return result;
        }

        public static IEnumerator GenerateComboAsync(string serverUrl, string dhsToken, 
            ParamsChat chat, ParamsTTS tts, ParamsATL atl,
            Action<ComboRequestReply> onComplete)
        {
            var request = GenerateComboRequest(chat, tts, atl);
            var json = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            Uri baseUri = new Uri(serverUrl);
            Uri reqUri = new Uri(baseUri, "/dh/api/v1/combo");

            var webRequest = UnityWebRequest.Put(reqUri, json);
            webRequest.method = "POST";
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + dhsToken);

            yield return webRequest.SendWebRequest();

            if (HasError(webRequest, "Combined message"))
            {
                onComplete?.Invoke(null);
                yield break;
            }

            var data = webRequest.downloadHandler.data;
            var reply = UnzipComboReply(data);

            onComplete?.Invoke(reply);
        }

        private static ComboRequestReply UnzipComboReply(byte[] data)
        {
            var reply = new ComboRequestReply();

            using (var stream = new MemoryStream(data))
            {
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Read))
                {
                    foreach (var entry in zip.Entries)
                    {
                        using (var file = entry.Open())
                        {
                            if (entry.Name.EndsWith(".wav"))
                            {
                                using (var copySteam = new MemoryStream())
                                {
                                    file.CopyTo(copySteam);
                                    var bytes = copySteam.ToArray();
                                    reply.wav = new WavFile(bytes);
                                }
                            }
                            else if (entry.Name.EndsWith(".json"))
                            {
                                var tr = new StreamReader(file);
                                reply.animation = tr.ReadToEnd();
                            }
                        }
                    }
                }
            }

            return reply;
        }
    }
}
