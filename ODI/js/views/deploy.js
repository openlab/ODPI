window.DeployView = Backbone.View.extend({

    initialize: function () {
        this.template = _.template(tpl.get('Deploy'));
    },

    events: {

    },

    render: function (eventName) {
        $(this.el).html(this.template());
        var self = this;
        _.each(window.selectedApps.models, function (app) {
            var v = self.getDeploymentItem(app);
            v.parentView = self;
            $(this.el).find('#deploy-items').append(v.render().el);
        }, this);
        return this;
    },
    getDeploymentItem: function (app) {
        if (window.azureConfig.manualInstall) {
            return new ManualItemView({ model: app });
        }
        else {
            return new DeployItemView({ model: app });
        }
    }

});