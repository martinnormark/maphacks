(function($) {

	function initializeMap() {
		var mapOptions = {
			zoom: 13,
			center: new google.maps.LatLng(53.463066, -2.290993),
			mapTypeId: google.maps.MapTypeId.ROADMAP
		};

		var map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);
	}

	function loadScript() {
		var script = document.createElement("script");
		script.type = "text/javascript";
		script.src = "http://maps.googleapis.com/maps/api/js?sensor=false&libraries=visualization&callback=initializeMap";
		document.body.appendChild(script);
	}

	window.initializeMap = initializeMap;
	window.onload = loadScript;
	
})(jQuery);