(function ($, Backbone, _, app) {

	"use strict";

	app.Views.MapView = Backbone.View.extend({

		el: "#map-canvas",

		initialize: function (options) {
			_.bindAll(this, "render");
		},

		render: function () {
			this.map = new google.maps.Map(this.el, {
				zoom: 3, 
				center: new google.maps.LatLng(35.88905007936091, 16.171875),
				mapTypeId: google.maps.MapTypeId.ROADMAP
			});

			google.maps.event.addListener(this.map, "click", function (event) {
				console.log({ Latitude: event.latLng.lat(), Longitude: event.latLng.lng() });
			});

			return this;
		},

		dropTweet: function (tweet) {
			if (tweet.geo) {

				var that = this,
					marker = new google.maps.Marker({
					animation: google.maps.Animation.DROP,
					position: new google.maps.LatLng(tweet.geo.coordinates.Latitude, tweet.geo.coordinates.Longitude),
					title: tweet.Text + " by @" + tweet.User.Name
				});

				var infowindow = new google.maps.InfoWindow({
					content: buildInitialInfoWindowContent(tweet)
				});

				marker.setMap(this.map);

				google.maps.event.addListener(marker, "click", function () {

					if (infowindow.isContentSet !== true) {

						infowindow.isContentSet = true;
					}

					infowindow.open(that.map, marker);
				});

				google.maps.event.addListener(infowindow, "domready", function () {

					var _infoWindow = this;
				});

			}
		}

	});

	function buildInitialInfoWindowContent (tweet) {
		var infoWindowContent = '<img style="height:40px;margin-right:10px;" src="' + tweet.User.ProfileImageUrlHttps + '" />'
		infoWindowContent += '<span style="color: green;">Tweet by <strong>@';
		infoWindowContent += tweet.User.Name + '</strong>:</span> ' + tweet.Text;

		return infoWindowContent;
	}

})(jQuery, Backbone, _, window.maphacks);

// Old Trafford: center: new google.maps.LatLng(53.463066, -2.290993)