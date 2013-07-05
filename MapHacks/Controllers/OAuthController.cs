using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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

		public ActionResult Callback(string oauth_token, string oauth_verifier)
		{
			var requestToken = new OAuthRequestToken { Token = oauth_token };

			// Step 3 - Exchange the Request Token for an Access Token
			TwitterService service = this.GetTwitterService();
			OAuthAccessToken accessToken = service.GetAccessToken(requestToken, oauth_verifier);

			// Step 4 - User authenticates using the Access Token
			service.AuthenticateWith(accessToken.Token, accessToken.TokenSecret);
			TwitterUser user = service.GetUserProfile(new GetUserProfileOptions());

			ViewBag.Message = string.Format("Your username is {0}", user.ScreenName);

			return View();
		}

		private string GetCallbackUrl()
		{
			return String.Format("{0}://{1}/Callback", Request.Url.Scheme, Request.Url.Host);
		}

		private TwitterService GetTwitterService()
		{
			TwitterService service = new TwitterService(this._consumerKey, this._consumerSecret);
			return service;
		}
	}
}