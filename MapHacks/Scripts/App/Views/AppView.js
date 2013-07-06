(function ($, Backbone, _, app) {

	"use strict";

	app.Views.AppView = Backbone.View.extend({

		el: "#app-view",
		
		events: {
			"click button.subscribe": "subscribeToUpdates"
		},

		initialize: function (options) {
			_.bindAll(this, "render", "renderUserDetails", "connectToHub", "subscribeToUpdates");

			this.user = options.user;
		},

		render: function () {
			this.mapView = new app.Views.MapView();
			this.mapView.render();

			this.searchView = new app.Views.SearchBarView();
			this.searchView.on("render_complete", this.renderUserDetails);
			this.searchView.render().$el.appendTo(this.$el);

			return this;
		},

		renderUserDetails: function () {
			if (!this.user) {
				var loginView = new app.Views.LoginView();
				loginView.render();
			}
			else {
				console.log("Hello %s", this.user.UserName);

				this.$el.append('<button class="btn btn-large subscribe">Subscribe to updates</button>');
				this.connectToHub();
			}
		},

		connectToHub: function () {
			var that = this,
				geoFeedHubProxy = $.connection.geoFeedHub;

			geoFeedHubProxy.client.addTweetToMap = function (tweet) {
				console.log(tweet);
				that.mapView.dropTweet(tweet);
			};

			geoFeedHubProxy.client.subscriptionEstablished = function (subscriptionId, channels) {
				console.log("Subscribed for %O, ID: %s", channels, subscriptionId);
			}

			$.connection.hub.start()
				.done(function (result) { console.log('Now connected, connection ID=' + $.connection.hub.id); })
				.fail(function(){ console.log('Could not Connect!'); });
		},

		subscribeToUpdates: function (event) {
			$.connection.geoFeedHub.server.subscribe(this.user, ["test", "old trafford"]);
		}

	});

})(jQuery, Backbone, _, window.maphacks);