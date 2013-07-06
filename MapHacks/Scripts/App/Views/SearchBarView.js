(function ($, Backbone, _, app) {

	"use strict";

	app.Views.SearchBarView = Backbone.View.extend({

		tagName: "div",

		className: "search-bar input-append animated fadeInDown",

		template: _.template([
			'<input type="text" id="search" class="input-xxlarge">',
			'<button id="searchButton" class="btn btn-large search"><i class="icon-search"></i></button>'
		].join('\n')),

		initialize: function (options) {
			_.bindAll(this, "render");
		},

		render: function () {
			var transEndEventNames = {
				'WebkitAnimation': 'webkitAnimationEnd',
				'MozAnimation': 'Animationend',
				'OAnimation': 'oAnimationEnd oAnimationend',
				'msAnimation': 'MSAnimationEnd',
				'Animation': 'Animationend'
			},
			transEndEventName = transEndEventNames[Modernizr.prefixed('animation')],
			that = this;

			this.$el.on(transEndEventName, function () {
				that.trigger("render_complete");
			});

			this.$el.attr("id", "panel").html(this.template({}));

			return this;
		}

	});

})(jQuery, Backbone, _, window.maphacks);