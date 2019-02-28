﻿using System.Runtime.Serialization;
using Elasticsearch.Net;

namespace Nest
{
	/// <summary>
	/// Limits the number of tokens that are indexed per document and field.
	/// </summary>
	public interface ILimitTokenCountTokenFilter : ITokenFilter
	{
		/// <summary>
		/// If set to true the filter exhaust the stream even if max_token_count tokens have been consumed already.
		/// </summary>
		[DataMember(Name ="consume_all_tokens")]
		[JsonFormatter(typeof(NullableStringBooleanFormatter))]
		bool? ConsumeAllTokens { get; set; }

		/// <summary>
		/// The maximum number of tokens that should be indexed per document and field.
		/// </summary>
		[DataMember(Name ="max_token_count")]
		[JsonFormatter(typeof(NullableStringIntFormatter))]
		int? MaxTokenCount { get; set; }
	}

	/// <inheritdoc />
	public class LimitTokenCountTokenFilter : TokenFilterBase, ILimitTokenCountTokenFilter
	{
		public LimitTokenCountTokenFilter() : base("limit") { }

		/// <inheritdoc />
		public bool? ConsumeAllTokens { get; set; }

		/// <inheritdoc />
		public int? MaxTokenCount { get; set; }
	}

	/// <inheritdoc />
	public class LimitTokenCountTokenFilterDescriptor
		: TokenFilterDescriptorBase<LimitTokenCountTokenFilterDescriptor, ILimitTokenCountTokenFilter>, ILimitTokenCountTokenFilter
	{
		protected override string Type => "limit";
		bool? ILimitTokenCountTokenFilter.ConsumeAllTokens { get; set; }

		int? ILimitTokenCountTokenFilter.MaxTokenCount { get; set; }

		/// <inheritdoc />
		public LimitTokenCountTokenFilterDescriptor ConsumeAllToken(bool? consumeAllTokens = true) =>
			Assign(a => a.ConsumeAllTokens = consumeAllTokens);

		/// <inheritdoc />
		public LimitTokenCountTokenFilterDescriptor MaxTokenCount(int? maxTokenCount) => Assign(a => a.MaxTokenCount = maxTokenCount);
	}
}
