using System;
using System.IO;
using ServiceStack.Serialization;
using ServiceStack.Text;
using ServiceStack.Web;

namespace ServiceStack
{
    public class JsonServiceClient
        : ServiceClientBase, IJsonServiceClient
    {
        public override string Format
        {
            get { return "json"; }
        }

        public JsonServiceClient()
        {
        }

        public JsonServiceClient(string baseUri) 
        {
            SetBaseUri(baseUri);
        }

        public JsonServiceClient(string syncReplyBaseUri, string asyncOneWayBaseUri) 
            : base(syncReplyBaseUri, asyncOneWayBaseUri)
        {
        }

        public override string ContentType
        {
            get { return String.Format("application/{0}", Format); }
        }

        public override void SerializeToStream(IRequest requestContext, object request, Stream stream)
        {
            JsonDataContractSerializer.Instance.SerializeToStream(request, stream);
        }

        public override T DeserializeFromStream<T>(Stream stream)
        {
            return JsonDataContractSerializer.Instance.DeserializeFromStream<T>(stream);
        }

        public override StreamDeserializerDelegate StreamDeserializer
        {
            get { return JsonSerializer.DeserializeFromStream; }
        }

        internal static JsonObject ParseObject(string json)
        {
            //modified by Yang Li
            return JsonObject.Parse(json);
            //using (__requestAccess())
            //{
            //    return JsonObject.Parse(json);
            //}
        }

        internal static T FromJson<T>(string json)
        {
            //modified by Yang Li
            return json.FromJson<T>();
            //using (__requestAccess())
            //{
            //    return json.FromJson<T>();
            //}
        }

        internal static string ToJson<T>(T o)
        {
            //modified by Yang Li
            return o.ToJson();
            //using (__requestAccess())
            //{
            //    return o.ToJson();
            //}
        }
    }
}