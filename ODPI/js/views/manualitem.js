window.ManualItemView = Backbone.View.extend({
    tagName: "div",

    events: {
        'click #btnmanual': 'manual'
    },

    initialize: function () {
        this.template = _.template(tpl.get('ManualItem'));
    },

    render: function (eventName) {
        $(this.el).html(this.template(this.model.toJSON()));

        //enable tooltips
        $("[rel=tooltip]").tooltip();

        return this;
    },

    manual: function () {
        console.log('getting manual download');
        var self = this;
        $.ajax({
            type: 'POST',
            url: '/Deploy/Manual',
            data:
                {
                    'key': window.azureConfig.key,
                    'data': JSON.stringify(self.model.settings[0].toJSON())
                },
            success: function (results) {
                if (results.Status == 0) {
                    window.location = "/Deploy/Download?file=" + results.Data;
                }
                else {
                    self.handleError(results);
                }
            }
        });

        return false;
    },

    handleError: function (message) {
        console.log(message);
    }

});