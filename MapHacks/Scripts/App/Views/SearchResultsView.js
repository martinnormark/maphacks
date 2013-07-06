(function ($, Backbone, _, app) {

	"use strict";

	app.Views.SearchResultsView = Backbone.View.extend({

		tagName: "div",

		className: "search-results",

		initialize: function (options) {
			_.bindAll(this, "render");
		},

		render: function () {
			return this;
		}

	});

})(jQuery, Backbone, _, window.maphacks);