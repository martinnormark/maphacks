(function ($, Backbone, _, app) {

	"use strict";

	app.Views.MapView = Backbone.View.extend({

		el: "#map-canvas",

		initialize: function (options) {
			_.bindAll(this, "render");
		},

		render: function () {
			var mapOptions = {
				zoom: 13,
				center: new google.maps.LatLng(53.463066, -2.290993),
				mapTypeId: google.maps.MapTypeId.ROADMAP
			};

			var map = new google.maps.Map(this.el, mapOptions);

			return this;
		}

	});

})(jQuery, Backbone, _, window.maphacks);