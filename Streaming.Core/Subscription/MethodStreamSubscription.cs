using Aspor.Streaming.Core;
using Aspor.Streaming.Core.Attributes;
using Aspor.Streaming.Core.Content;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Aspor.Streaming.Core.Subscription
{
    public class MethodStreamSubscription : IIStreamSubscription
    {

        private readonly MethodInfo _method;
        private readonly object _host;

        private Type _contentHolderType;

        public MethodStreamSubscription(string[] nodes, string[] noramlizedNodes, MethodInfo method, object host)
        {
            Nodes = nodes;
            NormalizedNodes = noramlizedNodes;
            _method = method;
            _host = host;
            Prepare();
        }

        public string[] NormalizedNodes { get; }

        public string[] Nodes { get; }

        private void Prepare()
        {
            ParameterInfo[] parameters = _method.GetParameters();
            foreach (ParameterInfo parameter in parameters)
            {
                FromContentAttribute bodyAttribute = parameter.GetCustomAttribute<FromContentAttribute>();
                if (bodyAttribute != null)
                {
                    if(typeof(IStreamContent).IsAssignableFrom(parameter.ParameterType))
                    {
                        _contentHolderType = parameter.ParameterType;
                    }
                }else if(parameter.ParameterType.IsGenericType && typeof(StreamContext<>).IsAssignableFrom(parameter.ParameterType.GetGenericTypeDefinition()))
                {
                    _contentHolderType = parameter.ParameterType.GetGenericArguments()[0];
                }
            }

        }

        public void Invoke(StreamData data)
        {
            IStreamContent content = CreateContent(data);

            StreamContext context = CreateContext(content);
            context.Data = data;
            context.Parameters = TopicMatcher.ExtractParameters(Nodes, data.Topic);

            ParameterInfo[] parameters = _method.GetParameters();
            object[] result = new object[parameters.Length];

            int index = 0;
            foreach(ParameterInfo parameter in parameters)
            {
                FromTopicAttribute topicAttribute = parameter.GetCustomAttribute<FromTopicAttribute>();
                if(topicAttribute != null) result[index] = context.Parameters[topicAttribute.Name ?? parameter.Name];

                FromContentAttribute bodyAttribute = parameter.GetCustomAttribute<FromContentAttribute>();
                if (bodyAttribute != null) result[index] = content;

                if (parameter.ParameterType == typeof(StreamContext))
                {
                    result[index] = context;
                }
                else if (parameter.ParameterType.IsGenericType && typeof(StreamContext<>).IsAssignableFrom(parameter.ParameterType.GetGenericTypeDefinition())) {
                    result[index] = context;
                } 
                else if (parameter.ParameterType == typeof(StreamData))
                {
                    result[index] = context.Data;
                }

                index++;
            }

            object callback = _method.Invoke(_host,result);
            if (callback != null && callback.GetType() == typeof(Task)) ((Task)callback).Wait();
        }

        private IStreamContent CreateContent(StreamData data)
        {
            if (_contentHolderType == null) return null;
            IStreamContent content = Activator.CreateInstance(_contentHolderType) as IStreamContent;
            content.Decode(data.Body);
            return content;
        }

        private StreamContext CreateContext(IStreamContent content)
        {
            if (content == null) return new StreamContext();
            Type d1 = typeof(StreamContext<>);
            Type[] typeArgs = { content .GetType()};
            Type contextType = d1.MakeGenericType(typeArgs);

            object result =  Activator.CreateInstance(contextType);

            contextType.GetProperty("Content").SetValue(result, content);

            return result as StreamContext;
        }
    }
}
