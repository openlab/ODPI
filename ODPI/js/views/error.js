window.ErrorView = Backbone.View.extend({
    
    initialize: function () {
        this.template = _.template(tpl.get('Error'));
    },

    events: {

    },

    render: function (eventName) {
        $(this.el).html(this.template(this.model));
        return this;
    }

});