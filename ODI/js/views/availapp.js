window.AvailAppView = Backbone.View.extend({
    tagName: "div",

    parentView: null,

    events: {
        "click .enable": "enable",
        "click .disable": "disable"
    },

    initialize: function () {
        this.template = _.template(tpl.get('AvailAppItem'));
        this.model.bind("change", this.render, this);
        this.model.bind("destroy", this.close, this);
    },

    render: function (eventName) {
        $(this.el).html(this.template(this.model.toJSON()));
        return this;
    },

    enable: function (event) {
        var attr = $(this.el).find('.enable').attr('disabled');
        if (typeof attr === 'undefined') {
            var id = this.model.id;
            $("#app-" + this.model.id).toggleClass('alert-success');
            $(this.el).find('.enable').attr('disabled', 'disabled');
            $(this.el).find('.disable').removeAttr('disabled');
            console.log(window.available.get(id));
            window.selectedApps.add(window.available.get(id));
            this.parentView.checkEnabled();
        }
    },

    disable: function (event) {
        var attr = $(this.el).find('.disable').attr('disabled');
        if (typeof attr === 'undefined') {
            $(this.el).find('.enable').removeAttr('disabled');
            $(this.el).find('.disable').attr('disabled', 'disabled');
            $("#app-" + this.model.attributes.Id).toggleClass('alert-success');
            window.selectedApps.remove(window.available.get(this.model.id));
            this.parentView.checkEnabled();
        }
    }

});