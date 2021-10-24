using System;
using System.Collections.Generic;

namespace Aspor.Streaming.Core
{
    public class TopicMatcher
    {

        public const string ANY = "*";

        public const string ALL = "#";

        public const char NODE_DELIMITER = '.';

        public const char PARAMETER_OPEN = '{';

        public const char PARAMETER_CLOSE = '}';

        public static string[] Compute(string topic)
        {
            return topic.Split(NODE_DELIMITER);
        }

        public static string[] Noramlize(string[] nodes)
        {
            string[] normalizedNodes = new string[nodes.Length];
            for (int i = 0; i < nodes.Length; i++)
            {
                string node = nodes[i];
                if (node.Length > 2 && node[0] == PARAMETER_OPEN && node[node.Length - 1] == PARAMETER_CLOSE)
                {
                    normalizedNodes[i] = ANY;
                }
                else
                {
                    normalizedNodes[i] = node;
                }
            }
            return normalizedNodes;
        }

        public static bool Matches(string[] nodes, string topic)
        {
            return Matches(nodes, topic.Split(NODE_DELIMITER));
        }

        public static bool Matches(string[] nodes, string[] topic)
        {
            if (topic.Length >= nodes.Length)
            {
                if (topic.Length > nodes.Length && !nodes[nodes.Length - 1].Equals(ALL)) return false;
                for (int i = 0; i < nodes.Length; i++)
                {
                    string node = nodes[i];
                    if (!node.Equals(topic[i]) && !node.Equals(ANY) && !node.Equals(ALL)) return false;
                }
                return true;
            }
            return false;
        }

        public static IDictionary<string, string> ExtractParameters(string[] nodes, string topic)
        {
            return ExtractParameters(nodes, topic.Split(NODE_DELIMITER));
        }

        public static IDictionary<string,string> ExtractParameters(string[] nodes, string[] topic)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            if (topic.Length >= nodes.Length)
            {
                for (int i = 0; i < nodes.Length; i++)
                {
                    string node = nodes[i];
                    if (node.Length > 2 && node[0] == PARAMETER_OPEN && node[node.Length - 1] == PARAMETER_CLOSE)
                    {
                        string key = node.Substring(1, node.Length - 2);
                        result[key] = topic[i];
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Unpossible match");
            }
            return result;
        }
    }
}
