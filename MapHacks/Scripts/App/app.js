(function ($) {

	"use strict";

	window.maphacks = {
		Models: {},
		Views: {}
	};

	function initializeApp() {
		var appView = new window.maphacks.Views.AppView({ user: window.currentUser });
		appView.render();
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