window.ConfigItemView = Backbone.View.extend({
    tagName: "div",
    initialize: function () {
        this.template = _.template(tpl.get('ConfigItem'));
        this.model.bind("change", this.render, this);
        this.model.bind("destroy", this.close, this);
    },

    parentElement: null,

    events: {
    },

    render: function (eventName) {
        if (this.model.attributes.Configs == null || this.model.attributes.Configs.length == 0)
            return this;

        if (this.model.settings == null) {
            this.model.settings = new Array();

            var self = this;
            $.each(this.model.attributes.Configs, function (index, typeName) {
                self.model.settings[index] = new Config({ name: self.model.attributes.Name, type: typeName });
            });

           
        }


        _.each(this.model.settings, function (c) {
            console.log("showing config type: " + c.attributes.type);
            //TODO: add a view here to handle saving of data
            var t = _.template(tpl.get(c.attributes.type.Template));
            $(this.el).append(t(c.toJSON()));
        }, this);

        for (var key in this.model.settings[0].attributes) {
            var value = this.model.settings[0].get(key);

            var input = $(this.el).find('#' + key);
            if (input != null)
                $(input).val(value);
        }

        //enable tooltips
        $("[rel=tooltip]").tooltip();
    },

    saveData: function (ind) {
        var self = this;
        $('.input-xlarge').each(function (index, ele) {
            window.selectedApps.models[ind].settings[0].set(ele.id, $(ele).val());
        });

    }

});