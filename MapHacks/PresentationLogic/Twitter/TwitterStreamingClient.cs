using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Streaming;
using Hammock.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using TweetSharp;

namespace MapHacks.PresentationLogic.Twitter
{
	public class TwitterStreamingClient
	{
		private readonly string _consumerKey;
		private readonly string _consumerSecret;
		private readonly string _oauthToken;
		private readonly string _oauthTokenSecret;
		private readonly TwitterClientInfo _info;

		private readonly RestClient _userStreamsClient;
		private readonly RestClient _publicStreamsClient;

		private readonly JsonSerializer _jsonSerializer;

		public TwitterStreamingClient(string clientName, string consumerKey, string consumerSecret, OAuthToken accessToken)
		{
			this._consumerKey = consumerKey;
			this._consumerSecret = consumerSecret;
			this._oauthToken = accessToken.Token;
			this._oauthTokenSecret = accessToken.TokenSecret;

			this._info = new TwitterClientInfo
			{
				ClientName = clientName,
				ClientUrl = "https://github.com/martinnormark/maphacks",
				ClientVersion = "0.1",
				ConsumerKey = consumerKey,
				ConsumerSecret = consumerSecret
			};

			this._userStreamsClient = new RestClient
			{
				Authority = "https://stream.twitter.com",
				VersionPath = "1.1",
				DecompressionMethods = DecompressionMethods.GZip,
				UserAgent = "StreamingTest",
				FollowRedirects = true
			};

			this._publicStreamsClient = new RestClient
			{
				Authority = "https://stream.twitter.com",
				VersionPath = "1.1",
				DecompressionMethods = DecompressionMethods.GZip,
				UserAgent = "StreamingTest",
				FollowRedirects = true
			};
		}

		/// <summary>
		/// Cancels pending streaming actions from this service.
		/// </summary>
		public virtual void CancelStreaming()
		{
			if (_userStreamsClient != null)
			{
				_userStreamsClient.CancelStreaming();
			}
			if (_publicStreamsClient != null)
			{
				_publicStreamsClient.CancelStreaming();
			}
		}

		/// <summary>
		/// Accesses an asynchronous Twitter filter stream indefinitely, until terminated.
		/// </summary>
		/// <seealso href="http://dev.twitter.com/pages/streaming_api_methods#statuses-filter" />
		/// <param name="action"></param>
		/// <returns></returns>
		public virtual IAsyncResult StreamFilter(string filter, Action<TwitterStreamArtifact, TwitterResponse> action)
		{
			var options = new StreamOptions { ResultsPerCallback = 1 };

			return WithHammockPublicStreaming(options, action, "statuses/filter.json");
		}

		/// <summary>
		/// Accesses an asynchronous Twitter user stream indefinitely, until terminated.
		/// </summary>
		/// <seealso href="http://dev.twitter.com/pages/user_streams" />
		/// <param name="action"></param>
		/// <returns></returns>
		public virtual IAsyncResult StreamUser(Action<TwitterStreamArtifact, TwitterResponse> action)
		{
			var options = new StreamOptions { ResultsPerCallback = 1 };

			return WithHammockUserStreaming(options, action, "user.json");
		}

		private IAsyncResult WithHammockUserStreaming<T>(StreamOptions options, Action<T, TwitterResponse> action, string path) where T : class
		{
			var request = PrepareHammockQuery(path);

			return WithHammockStreamingImpl(_userStreamsClient, request, options, action);
		}

		private IAsyncResult WithHammockPublicStreaming<T>(StreamOptions options, Action<T, TwitterResponse> action, string path) where T : class
		{
			var request = PrepareHammockQuery(path);

			return WithHammockStreamingImpl(_publicStreamsClient, request, options, action);
		}

		private IAsyncResult WithHammockStreamingImpl<T>(RestClient client, RestRequest request, StreamOptions options, Action<T, TwitterResponse> action)
		{
			request.StreamOptions = options;
			request.Method = WebMethod.Post;
			//request.AddParameter("track", "twitter");
			request.AddParameter("locations", "-122.75,36.8,-121.75,37.8");

			return client.BeginRequest(request, new RestCallback<T>((req, resp, state) =>
			{
				Exception exception;
				var entity = TryAsyncResponse(() =>
								{
									return JsonConvert.DeserializeObject<T>(resp.Content);
								},
								out exception);
				action(entity, new TwitterResponse(resp, exception));
			}));
		}

		private RestRequest PrepareHammockQuery(string path)
		{
			RestRequest request;

			if (string.IsNullOrEmpty(_oauthToken) || string.IsNullOrEmpty(_oauthTokenSecret))
			{
				throw new ArgumentException("Access tokens are missing!");
			}
			else
			{
				var args = new FunctionArguments
				{
					ConsumerKey = _consumerKey,
					ConsumerSecret = _consumerSecret,
					Token = _oauthToken,
					TokenSecret = _oauthTokenSecret
				};

				request = _protectedResourceQuery.Invoke(args);
			}

			request.Path = path;

			SetTwitterClientInfo(request);

			request.TraceEnabled = false;

			return request;
		}

		private static T TryAsyncResponse<T>(Func<T> action, out Exception exception)
		{
			exception = null;
			var entity = default(T);

			try
			{
				entity = action();
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			return entity;
		}

		private void SetTwitterClientInfo(RestBase request)
		{
			if (_info == null) return;

			if (!String.IsNullOrWhiteSpace(_info.ClientName))
			{
				request.AddHeader("X-Twitter-Name", _info.ClientName);
				request.UserAgent = _info.ClientName;
			}

			if (!String.IsNullOrWhiteSpace(_info.ClientVersion))
			{
				request.AddHeader("X-Twitter-Version", _info.ClientVersion);
			}

			if (!String.IsNullOrWhiteSpace(_info.ClientUrl))
			{
				request.AddHeader("X-Twitter-URL", _info.ClientUrl);
			}
		}

		private readonly Func<FunctionArguments, RestRequest> _protectedResourceQuery
						= args =>
						{
							var request = new RestRequest
							{
								Credentials = new OAuthCredentials
								{
									Type = OAuthType.ProtectedResource,
									SignatureMethod = OAuthSignatureMethod.HmacSha1,
									ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
									ConsumerKey = args.ConsumerKey,
									ConsumerSecret = args.ConsumerSecret,
									Token = args.Token,
									TokenSecret = args.TokenSecret,
								}
							};

							return request;
						};

		private class FunctionArguments
		{
			public string ConsumerKey { get; set; }
			public string ConsumerSecret { get; set; }
			public string Token { get; set; }
			public string TokenSecret { get; set; }
			public string Verifier { get; set; }
			public string Username { get; set; }
			public string Password { get; set; }
		}
	}
}