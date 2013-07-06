(function ($) {

	"use strict";

	window.maphacks = {
		Models: {},
		Views: {}
	};

	function initializeApp() {

		var appView = new window.maphacks.Views.AppView();
		appView.render();

		/*var geoFeedHubProxy = $.connection.geoFeedHub;

		geoFeedHubProxy.client.addTweetToMap = function (tweet) {
			console.log(tweet);
		};

		$.connection.hub.start()
				.done(function (result) { console.log('Now connected, connection ID=' + $.connection.hub.id); })
				.fail(function(){ console.log('Could not Connect!'); });*/
	}

	function loadScript() {
		var script = document.createElement("script");
		script.type = "text/javascript";
		script.src = "http://maps.googleapis.com/maps/api/js?sensor=false&libraries=visualization&callback=initializeApp";
		document.body.appendChild(script);
	}

	window.initializeApp = initializeApp;
	window.onload = loadScript;

})(jQuery);