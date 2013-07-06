using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using MapHacks.Models;
using MapHacks.PresentationLogic.Twitter;
using System.Configuration;
using System.Web.SessionState;

namespace MapHacks.Hubs
{
	public class GeoFeedHub : Hub, IRequiresSessionState
	{
		private readonly string _consumerKey;
		private readonly string _consumerSecret;

		public GeoFeedHub()
		{
			this._consumerKey = ConfigurationManager.AppSettings.Get("Twitter:ConsumerKey");
			this._consumerSecret = ConfigurationManager.AppSettings.Get("Twitter:ConsumerSecret");
		}

		public void Subscribe(UserDetailsViewModel userDetails, string[] channels)
		{
			Guid subscriptionId = Guid.NewGuid();

			RealTimeStreamNotifier notifier = new RealTimeStreamNotifier("Map hacks", this._consumerKey, this._consumerSecret, userDetails.AccessToken, userDetails.AccessTokenSecret);
			IAsyncResult result = notifier.SubscribeForFilter("test");

			Clients.Caller.subscriptionEstablished(subscriptionId, channels);
		}

		public override Task OnConnected()
		{
			return base.OnConnected();
		}

		public override Task OnDisconnected()
		{
			return base.OnDisconnected();
		}

		public override Task OnReconnected()
		{
			return base.OnReconnected();
		}
	}
}