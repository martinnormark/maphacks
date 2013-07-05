using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace MapHacks.Hubs
{
	public class GeoFeedHub : Hub
	{
		public void Subscribe(string username, string[] channels)
		{
			Clients.Caller.subscriptionEstablished(Guid.NewGuid(), channels);
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