(function ($, Backbone, _, app) {

	"use strict";

	app.Views.AppView = Backbone.View.extend({

		el: "#app-view",

		initialize: function (options) {
			_.bindAll(this, "render", "renderAuthBox");
		},

		render: function () {
			var mapView = new app.Views.MapView();
			mapView.render();

			var searchView = new app.Views.SearchBarView();
			searchView.on("render_complete", this.renderAuthBox);
			searchView.render().$el.appendTo(this.$el);

			return this;
		},

		renderAuthBox: function () {
			console.log("ds");
		}

	});

})(jQuery, Backbone, _, window.maphacks);