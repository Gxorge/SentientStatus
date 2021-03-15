using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SentientStatus
{
    public class SentientStatus
    {
        public List<StatusData> data;
        public RandomMessageData randomData;
        public Timer worker;
        public StatusData current;

        private string authorization;

        public void Start()
        {
            // Load the authorization token
            if (!File.Exists("token.txt"))
            {
                Console.WriteLine("token file does not exist!");
                return;
            }
                
            string[] tokenLines = File.ReadAllLines("token.txt");
            authorization = tokenLines[0];

            ParseSetMessage();
            ParseRandomMessage();
            
            var start = TimeSpan.Zero;
            var interval = TimeSpan.FromSeconds(1800); //1800
            Console.WriteLine("Starting worker.");
            worker = new Timer((e) => {

                StatusData sd = GetRandom();
                bool result = Call(sd.message, sd.emoji).Result;

            }, null, start, interval);
        }


        public void Stop()
        {
            Console.WriteLine("Stopping worker.");
            worker.Dispose();
        }

        public async Task<bool> Call(string msg, string emoji)
        {
            string url = "https://canary.discord.com/api/v8/users/@me/settings";

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("authorization", authorization);

            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, url)
            {
                Content = new StringContent("{\"custom_status\":{\"text\":\"" + msg + "\",\"emoji_name\":\"" + Helper.GetStringEmoji(emoji) + "\"}}", Encoding.UTF8, "application/json")
            };

            HttpResponseMessage hrm = await client.SendAsync(request);
            if (hrm.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"updated to {msg} with emoji {emoji}");
                return true;
            }
            else
            {
                Console.WriteLine("no work :( " + hrm.StatusCode.ToString());
                return false;
            }
            //Console.ReadLine();
        }

        public void ParseSetMessage()
        {
            using (StreamReader r = new StreamReader("data_const.json"))
            {
                string json = r.ReadToEnd();
                SetMessageData d = JsonConvert.DeserializeObject<SetMessageData>(json);
                data = d.status;
            }
        }

        public void ParseRandomMessage()
        {
            using (StreamReader r = new StreamReader("data_rand.json"))
            {
                string json = r.ReadToEnd();
                randomData = JsonConvert.DeserializeObject<RandomMessageData>(json);
            }
        }

        public StatusData GetRandom()
        {
            var random = new Random();
            StatusData d = null;
            
            if (false /*random.Next(2) == 0*/) // Random Status!
            {
                ConnectorData connector = randomData.connectors[random.Next(randomData.connectors.Count)];

                d = new StatusData();
                d.emoji = "robot";
                d.message = Helper.GetRandomSentence(connector, randomData);
                Console.WriteLine(d.message);
            }
            else // Normal status
            {
                while (d == null || d == current)
                    d = data[random.Next(data.Count)];
            }

            return d;
        }
    }
}
