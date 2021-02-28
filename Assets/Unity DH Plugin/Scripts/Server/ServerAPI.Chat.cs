using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace ARVRLab.UnityDH_Plugin.Server
{
    public partial class ServerAPI
    {
        public static Request<AttributesChat> GenerateChatRequest(string userPhrase)
        {
            var request = new Request<AttributesChat>
            {
                data = new Data<AttributesChat>
                {
                    id = Guid.NewGuid().ToString(),
                    type = "chat",
                    attributes = new AttributesChat()
                    {
                        chat = new ParamsChat()
                        {
                            history = new string[] { userPhrase }
                        }
                    }
                }
            };

            return request;
        }

        public static IEnumerator GenerateChatMsgAsync(string serverUrl, string dhsToken, string userPhrase, Action<string> onComplete)
        {
            var chatMessage = GenerateChatRequest(userPhrase);
            var msgJson = JsonConvert.SerializeObject(chatMessage, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            Uri baseUri = new Uri(serverUrl);
            Uri reqUri = new Uri(baseUri, "/dh/api/v1/chat");

            var webRequest = UnityWebRequest.Put(reqUri, msgJson);
            webRequest.method = "POST";
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + dhsToken);
            yield return webRequest.SendWebRequest();

            if (HasError(webRequest, "Chatbot"))
            {
                onComplete?.Invoke(null);
                yield break;
            }

            var answerJson = webRequest.downloadHandler.text;
            var answer = JsonUtility.FromJson<DlgAnswer>(answerJson);
            onComplete?.Invoke(answer.reply);
        }
    }
}