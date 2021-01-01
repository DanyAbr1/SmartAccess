using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartAccess.Api.Helper
{
    public static class NotificationsSend
    {
        public static async Task SendNotificationJSON( string title, string body)
        {
            string tokenDevice = "cT_l3d7TqoQ:APA91bHzmT_b4Wvz7itN1BXR4UZdnH7brWBVeS86wz-Ig5Nr9qh8dce1I_A65PMEMcIdQ_h7GZXQK5zt8j4KxGYIvonRoSN6SaSuzOn4x2PF9mkCtrZinXMVupXERkwDlfXgrarErjQo";
            string SERVER_KEY_TOKEN = "AAAA3LK3p_U:APA91bGUI6DLBs0rlUpkZjpgLPMszjw5VcvUTwQ8nWCVTsL4MHqCnx8fKke9waN8t61zrivYKtlMp_n4gj_RugY6mwzL-bIM0cibwtQhqskUEr5Wdwj0KQoy-hzplCWucbeaTXxr7rVD";
            var SENDER_ID = "947891185653";

            WebRequest tRequest;
            tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = " application/json";

            tRequest.Headers.Add(string.Format("Authorization: key={0}", SERVER_KEY_TOKEN));
            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

            var a = new
            {
                notification = new
                {
                    title,
                    body
                },
                to = tokenDevice
            };

            byte[] byteArray = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(a));
            tRequest.ContentLength = byteArray.Length;

            Stream dataStream = await tRequest.GetRequestStreamAsync();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse tResponse = await tRequest.GetResponseAsync();
            dataStream = tResponse.GetResponseStream();

            StreamReader tReader = new StreamReader(dataStream);
            string sResponseFromServer = tReader.ReadToEnd();

            tReader.Close();
            dataStream.Close();
            tResponse.Close();

        }


        public static async Task<bool> SendNotificationToAll(string tittle, string bodyMesage)
        {
            try
            {
                // Get the server key from FCM console
                var serverKey = string.Format("key={0}", "AAAA3LK3p_U:APA91bGUI6DLBs0rlUpkZjpgLPMszjw5VcvUTwQ8nWCVTsL4MHqCnx8fKke9waN8t61zrivYKtlMp_n4gj_RugY6mwzL-bIM0cibwtQhqskUEr5Wdwj0KQoy-hzplCWucbeaTXxr7rVD");

                // Get the sender id from FCM console
                var senderId = string.Format("id={0}", "947891185653");

                var data = new
                {
                    to = "/topics/SmartAcces", // Recipient device token
                    notification = new { title = tittle, body = bodyMesage }
                };

                // Using Newtonsoft.Json
                var jsonBody = JsonConvert.SerializeObject(data);

                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send"))
                {
                    httpRequest.Headers.TryAddWithoutValidation("Authorization", serverKey);
                    httpRequest.Headers.TryAddWithoutValidation("Sender", senderId);
                    httpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    using (var httpClient = new HttpClient())
                    {
                        var result = await httpClient.SendAsync(httpRequest);

                        if (result.IsSuccessStatusCode)
                        {
                            return true;
                        }
                        else
                        {
                            // Use result.StatusCode to handle failure
                            // Your custom error handler here
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

            return true;


            //WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            //tRequest.Method = "post";
            ////serverKey - Key from Firebase cloud messaging server  
            //tRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAA3LK3p_U:APA91bGUI6DLBs0rlUpkZjpgLPMszjw5VcvUTwQ8nWCVTsL4MHqCnx8fKke9waN8t61zrivYKtlMp_n4gj_RugY6mwzL-bIM0cibwtQhqskUEr5Wdwj0KQoy-hzplCWucbeaTXxr7rVD"));
            ////Sender Id - From firebase project setting                  
            //tRequest.Headers.Add(string.Format("Sender: id={0}", "947891185653"));
            //tRequest.ContentType = "application/json";
            //var payload = new
            //{
            //    to = "my_notification_channel",
            //    priority = "high",
            //    content_available = true,
            //    notification = new
            //    {
            //        body = bodyMesage,
            //        title = tittle,

            //    }
            //    ,
            //    data = new
            //    {
            //        key1 = "value1",
            //        key2 = "value2"
            //    }

            //};

            //string postbody = JsonConvert.SerializeObject(payload).ToString();
            //Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
            //tRequest.ContentLength = byteArray.Length;
            //using (Stream dataStream = tRequest.GetRequestStream())
            //{
            //    dataStream.Write(byteArray, 0, byteArray.Length);
            //    using (WebResponse tResponse =await tRequest.GetResponseAsync())
            //    {
            //        using (Stream dataStreamResponse = tResponse.GetResponseStream())
            //        {
            //            if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
            //                {
            //                    String sResponseFromServer = tReader.ReadToEnd();
            //                    //result.Response = sResponseFromServer;
            //                }
            //        }
            //    }
            //}
        }
    }
}
