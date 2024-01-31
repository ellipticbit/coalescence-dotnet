﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using EllipticBit.Coalescence.Shared;
using EllipticBit.Coalescence.Shared.Request;

namespace EllipticBit.Coalescence.Request
{
	internal sealed class CoalescenceRequest : ICoalescenceRequest
	{
		private readonly IHttpClientFactory httpClientFactory;
		private readonly IEnumerable<ICoalescenceAuthentication> authenticators;
		private readonly CoalescenceRequestOptions options;

		public CoalescenceRequest(IHttpClientFactory httpClientFactory, IEnumerable<ICoalescenceAuthentication> authenticators, CoalescenceRequestOptions options) {
			this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory), "Must specify an IHttpClientFactory instance to use for this request.");
			this.authenticators = authenticators ?? throw new ArgumentNullException(nameof(options), "Must provide at least one authenticator to use for this request.");
			this.options = options ?? throw new ArgumentNullException(nameof(options), "Must specify an options instance to use for this request.");
		}

		public ICoalescenceRequestBuilder Get() {
			return new CoalescenceRequestBuilder(HttpMethod.Get, httpClientFactory, authenticators, options);
		}

		public ICoalescenceRequestBuilder Put() {
			return new CoalescenceRequestBuilder(HttpMethod.Put, httpClientFactory, authenticators, options);
		}

		public ICoalescenceRequestBuilder Post() {
			return new CoalescenceRequestBuilder(HttpMethod.Post, httpClientFactory, authenticators, options);
		}

		public ICoalescenceRequestBuilder Patch() {
			return new CoalescenceRequestBuilder(new HttpMethod("PATCH"), httpClientFactory, authenticators, options);
		}

		public ICoalescenceRequestBuilder Delete() {
			return new CoalescenceRequestBuilder(HttpMethod.Delete, httpClientFactory, authenticators, options);
		}

		public ICoalescenceRequestBuilder Head() {
			return new CoalescenceRequestBuilder(HttpMethod.Head, httpClientFactory, authenticators, options);
		}
	}
}
