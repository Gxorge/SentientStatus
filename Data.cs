using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SentientStatus
{
    public class SetMessageData
    {
        public List<StatusData> status { get; set; }
    }

    public class RandomMessageData
    {
        public List<string> objects { get; set; }
        public List<string> people { get; set; }
        public List<string> places { get; set; }
        public List<ConnectorData> connectors { get; set; }
    }

    public class SetDayMessageData
    {
        public List<DayStatusData> status { get; set; }
    }

    public class ConnectorData
    {
        public string connector;
        
        [JsonConverter(typeof(StringEnumConverter))]
        public ConnectorType firstType;
        [JsonConverter(typeof(StringEnumConverter))]
        public ConnectorType secondType;
        
        [JsonProperty (ItemConverterType = typeof(StringEnumConverter))]
        public List<ConnectorConditions> conditions;

        public enum ConnectorType
        {
            Objects,
            People,
            Places
        }

        public enum ConnectorConditions
        {
            NoTypeRepeat,
            OnlyOneTypeStart,
            OnlyOneTypeEnd
        }
    }

    public class StatusData
    {
        public string emoji;
        public string message;
    }

    public class DayStatusData
    {
        public int day;
        public int month;
        public StatusData statusData;
    }

    public class Helper
    {
        private static Random random = new Random();
        
        public static string GetRandomSentence(ConnectorData connector, RandomMessageData randomData)
        {
            string final = null;
            
            bool noRepeat = connector.conditions.Contains(ConnectorData.ConnectorConditions.NoTypeRepeat);
            bool onlyOneType = connector.conditions.Contains(ConnectorData.ConnectorConditions.OnlyOneTypeStart) ||
                               connector.conditions.Contains(ConnectorData.ConnectorConditions.OnlyOneTypeEnd);

            string firstPart = GetRandomDataPart(connector.firstType, randomData);
            string middlePart = connector.connector;
            string secondPart = GetRandomDataPart(connector.secondType, randomData);
            if (noRepeat && secondPart == firstPart)
            {
                while (secondPart == firstPart)
                {
                    Console.WriteLine($"{secondPart} compared to {firstPart}");
                    secondPart = GetRandomDataPart(connector.secondType, randomData);
                }
            }

            if (onlyOneType)
            {
                if (connector.conditions.Contains(ConnectorData.ConnectorConditions.OnlyOneTypeStart))
                    final = firstPart + " " + middlePart;
                else
                    final = middlePart + " " + firstPart;
            }
            else
            {
                final = firstPart + " " + middlePart + " " + secondPart;
            }

            return final;
        }
        
        public static string GetRandomDataPart(ConnectorData.ConnectorType type, RandomMessageData data)
        {
            string toReturn = "unsupported";
            switch (type)
            {
                case ConnectorData.ConnectorType.Objects:
                {
                    var yes = random.Next(data.objects.Count);
                    Console.WriteLine($"DEBUG OBJECTS:\nrand int {yes}\nobjects size {data.objects.Count}");
                    toReturn = data.objects[yes];
                    break;
                }
                case ConnectorData.ConnectorType.People:
                {
                    var yes = random.Next(data.people.Count);
                    Console.WriteLine($"DEBUG PEOPLE:\nrand int {yes}\npeople size {data.people.Count}");
                    toReturn = data.people[yes];
                    break;
                }
                case ConnectorData.ConnectorType.Places:
                {
                    var yes = random.Next(data.places.Count);
                    Console.WriteLine($"DEBUG PLACES:\nrand int {yes}\nplaces size {data.places.Count}");
                    toReturn = data.places[yes];
                    break;
                }
            }

            return toReturn;
        }

        public static string GetStringEmoji(string emoji)
        {
            switch (emoji)
            {
                case "trans":
                    return "🏳️‍⚧️";
                case "pensive":
                    return "😔";
                case "smile":
                    return "😄";
                case "kitty":
                    return "🐱";
                case "kitty_smirk":
                    return "😼";
                case "neutral":
                    return "😐";
                case "heart_orange":
                    return "🧡";
                case "heart_purple":
                    return "💜";
                case "heart_blue":
                    return "💙";
                case "heart_green":
                    return "💚";
                case "heart_sparkle":
                    return "💖";
                case "heart_face":
                    return "🥰";
                case "olive":
                    return "🫒";
                case "pineapple":
                    return "🍍";
                case "cool":
                    return "😎";
                case "snip":
                    return "✂️";
                case "pc":
                    return "🖥️";
                case "sparkle":
                    return "✨";
                case "rainbow":
                    return "🏳️‍🌈";
                case "trumpet":
                    return "🎺";
                case "robot":
                    return "🤖";
                case "moon":
                    return "🌑";
                case "uk":
                    return "🇬🇧";
                case "pt":
                    return "🇵🇹";
                case "egg":
                    return "🥚";
                case "fire":
                    return "🔥";
                case "skull":
                    return "💀";
                case "spinny_light":
                    return "🚨";
                case "hatching_chick":
                    return "🐣";
                default:
                    return "🍉";
            }
        }
    }
}
