using Aspor.Validation;
using System.Collections.Generic;

namespace Aspor.Streaming
{
    public class StreamData
    {

        public ValidationAction Action { get; set; }

        public string Topic { get; set; }

        public object Instance { get; set; }

        public IDictionary<string, object> AdditionalProperties { get; set; }

        public IEnumerable<string> AffectedProperties { get; set; }

    }
}
