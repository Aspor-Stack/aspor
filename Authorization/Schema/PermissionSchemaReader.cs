using Aspor.Authorization.Schema.Conditions;
using Aspor.Authorization.Schema.Element;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Aspor.Authorization.Schema
{
    public class PermissionSchemaReader
    {

        public static PermissionSchema ReadFromFile(string path)
        {
            return Read(File.ReadAllText(path));
        }

        public async static Task<PermissionSchema> ReadFromFileAsync(string path)
        {
            return Read(await File.ReadAllTextAsync(path));
        }

        public static PermissionSchema Read(string input)
        {
            JToken json = JToken.Parse(input);
            return (PermissionSchema)Read(null, json);
        }

        private static IPermissionElement Read(string previousKey, JToken token)
        {
            if (token is JValue jValue)
            {
                return new ValuePermissionElement(jValue.Value.ToString());
            }
            else
            {
                IList<IPermissionElement> nodes = new List<IPermissionElement>();

                if (token is JArray jArray)
                {
                    foreach (var entry in jArray)
                    {
                        nodes.Add(Read(null, entry));
                    }
                }
                else if (token is JObject jObject)
                {
                    foreach (var entry in jObject)
                    {
                        nodes.Add(Read(entry.Key, entry.Value));
                    }
                }

                if (previousKey == null) return new PermissionSchema(nodes);
                else
                {
                    IPermissionCondition condition = PermissionConditionRegistry.Parse(previousKey);
                    return new ConditionPermissionElement(condition, nodes);
                }
            }
        }

    }
}
