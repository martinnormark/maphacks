(function ($, Backbone, _, app) {

	"use strict";

	app.Views.LoginView = Backbone.View.extend({

		tagName: "div",

		className: "login-view modal hide fade",

		template: _.template([
			'<div class="modal-header">',
				'<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>',
				'<h3>Sign in with Twitter</h3>',
			'</div>',
			'<div class="modal-body">',
				'<p>You need to login in order to view live updates from Twitter.</p>',
				'<p><a href="/OAuth/Authorize" class="btn btn-large btn-primary"><i class="icon-twitter"></i> Sign in with Twitter</a></p>',
			'</div>'
		].join("\n")),

		initialize: function (options) {
			_.bindAll(this, "render");
		},

		render: function () {
			this.$el.html(this.template({}));
			this.$el.appendTo($(document.body));

			this.$el.modal();

			return this;
		}

	});

})(jQuery, Backbone, _, window.maphacks);