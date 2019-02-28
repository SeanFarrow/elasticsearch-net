﻿using System.Runtime.Serialization;
using Elasticsearch.Net;

namespace Nest
{
	[InterfaceDataContract]
	[ReadAs(typeof(PutWatchResponse))]
	public interface IPutWatchResponse : IResponse
	{
		[DataMember(Name = "created")]
		bool Created { get; }

		[DataMember(Name = "_id")]
		string Id { get; }

		[DataMember(Name = "_version")]
		int Version { get; }
	}

	public class PutWatchResponse : ResponseBase, IPutWatchResponse
	{
		public bool Created { get; internal set; }
		public string Id { get; internal set; }

		public int Version { get; internal set; }
	}
}
