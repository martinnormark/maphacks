using MapHacks.Hubs;
using MapHacks.PresentationLogic.Twitter;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TweetSharp;

namespace MapHacks.Controllers
{
	public class OAuthController : Controller
	{
		private readonly string _consumerKey;
		private readonly string _consumerSecret;

		public OAuthController()
		{
			this._consumerKey = ConfigurationManager.AppSettings.Get("Twitter:ConsumerKey");
			this._consumerSecret = ConfigurationManager.AppSettings.Get("Twitter:ConsumerSecret");
		}

		//
		// GET: /OAuth/
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Authorize()
		{
			// Step 1 - Retrieve an OAuth Request Token
			TwitterService service = this.GetTwitterService();

			// This is the registered callback URL
			OAuthRequestToken requestToken = service.GetRequestToken(this.GetCallbackUrl());

			// Step 2 - Redirect to the OAuth Authorization URL
			Uri uri = service.GetAuthorizationUri(requestToken);

			return new RedirectResult(uri.ToString(), false);
		}

		public async Task<ActionResult> Callback(string oauth_token, string oauth_verifier)
		{
			var requestToken = new OAuthRequestToken { Token = oauth_token };

			// Step 3 - Exchange the Request Token for an Access Token
			TwitterService service = this.GetTwitterService();
			OAuthAccessToken accessToken = service.GetAccessToken(requestToken, oauth_verifier);

			// Step 4 - User authenticates using the Access Token
			service.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);
			TwitterUser user = service.GetUserProfile(new GetUserProfileOptions());

			TwitterStreamingClient streamClient = new TwitterStreamingClient("Map hacks", this._consumerKey, this._consumerSecret, new Hammock.Authentication.OAuth.OAuthToken { Token = accessToken.Token, TokenSecret = accessToken.TokenSecret });
			IAsyncResult result = streamClient.StreamFilter("track=twitter", (artifact, response) =>
			{
				TwitterStatus status = service.Deserialize<TwitterStatus>(response.Response);

				IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<GeoFeedHub>();
				hubContext.Clients.All.addTweetToMap(status);
			});

			IAsyncResult asyncResult = await Task.FromResult(result);

			ViewBag.Message = string.Format("Your username is {0}", user.ScreenName);

			return RedirectToAction("Index", "Home");
		}

		private string GetCallbackUrl()
		{
			return String.Format("{0}://{1}:{2}/oauth/callback", Request.Url.Scheme, Request.Url.Host, Request.Url.Port);
		}

		private TwitterService GetTwitterService()
		{
			TwitterService service = new TwitterService(this._consumerKey, this._consumerSecret);

			return service;
		}
	}
}