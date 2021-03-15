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

    public class Helper
    {
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
            var random = new Random();
            switch (type)
            {
                case ConnectorData.ConnectorType.Objects:
                {
                    toReturn = data.objects[random.Next(data.objects.Count)];
                    break;
                }
                case ConnectorData.ConnectorType.People:
                {
                    toReturn = data.people[random.Next(data.people.Count)];
                    break;
                }
                case ConnectorData.ConnectorType.Places:
                {
                    toReturn = data.places[random.Next(data.places.Count)];
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
                default:
                    return "🍉";
            }
        }
    }
}
