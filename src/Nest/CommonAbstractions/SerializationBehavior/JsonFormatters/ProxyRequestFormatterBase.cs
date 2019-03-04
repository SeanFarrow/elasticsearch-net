using System.Reflection;
using Elasticsearch.Net;

namespace Nest
{
	internal abstract class ProxyRequestFormatterBase<TRequestInterface, TRequest> : IJsonFormatter<TRequestInterface>
		where TRequestInterface : IProxyRequest
		where TRequest : TRequestInterface
	{
		public TRequestInterface Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
		{
			// TODO: Look at optimizing this. It looks like this could be replaced with SourceFormatter<T> on Document and a serialization ctor
			var segment = reader.ReadNextBlockSegment();
			var settings = formatterResolver.GetConnectionSettings();
			using (var ms = settings.MemoryStreamFactory.Create(segment.Array, segment.Offset, segment.Count))
			{
				//not optimized but deserializing create requests is far from common practice
				var genericType = typeof(TRequest).GetTypeInfo().GenericTypeArguments[0];
				var o = settings.SourceSerializer.Deserialize(genericType, ms);
				var path = typeof(DocumentPath<>).CreateGenericInstance(genericType, o);
				// index, type and id are optional parameters on _genericRequestType but need to be passed to construct through reflection

				// TRequest might be an open or closed generic type
				var request = typeof(TRequest).IsGenericTypeDefinition
					? (TRequest)typeof(TRequest).CreateGenericInstance(genericType, path, null, null, null)
					: (TRequest)typeof(TRequest).CreateInstance(path, null, null, null);

				return request;
			}
		}

		public void Serialize(ref JsonWriter writer, TRequestInterface value, IJsonFormatterResolver formatterResolver)
		{
			var untypedDocumentRequest = (IProxyRequest)value;

			// TODO: Allow formatting
			//var f = writer.Formatting == Formatting.Indented ? Indented : None;

			var settings = formatterResolver.GetConnectionSettings();
			var serializer = settings.SourceSerializer;

			using (var ms = settings.MemoryStreamFactory.Create())
			{
				untypedDocumentRequest.WriteJson(serializer, ms, SerializationFormatting.None);
				var v = ms.ToArray();
				writer.WriteRaw(v);
			}
		}
	}
}