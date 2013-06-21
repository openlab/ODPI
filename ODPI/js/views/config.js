window.ConfigView = Backbone.View.extend({

    initialize: function () {
        this.template = _.template(tpl.get('Config'));
    },
    currentApp: 0,
    events: {
        'click #btnBack': 'back',
        'click #btnContinue': 'next'
    },

    render: function (eventName) {
        console.log("We are rendering the config for selected app #" + this.currentApp);
        var app = window.selectedApps.models[this.currentApp];

        $(this.el).html(this.template(app.toJSON()));
        this.renderItem(app);

        setTimeout(function () {
            //enable tooltips
            $("[rel=tooltip]").tooltip();
        }, 500);
        return this;
    },

    renderItem: function (app) {
        this.configItemView = new ConfigItemView({ model: app });
        this.configItemView.render();

        $(this.el).find('#config-container').html(this.configItemView.el);


    },

    next: function () {
        this.configItemView.saveData(this.currentApp);
        var self = this;
        $.ajax({
            type: 'POST',
            url: '/App/Validate',
            data:
                {
                    'data': JSON.stringify(window.selectedApps.models[self.currentApp].settings[0].toJSON()),
                    'app': window.selectedApps.models[this.currentApp].attributes.Name
                },
            success: function (results) {
                if (results.Status == 0) {
                    window.app.hideError();
                    console.log(results);
                    if (self.currentApp < window.selectedApps.length - 1) {
                        self.currentApp++;
                        var app = window.selectedApps.models[self.currentApp];
                        $('#config-title').html('Configuring ' + app.attributes.Name + ":");
                        $('#config-help-text').html(app.attributes.ConfigHelpText);
                        console.log(app.attributes.ConfigHelpText);
                        self.renderItem(app);
                    }
                    else {
                        console.log('Reached the end of the apps to configure');
                        window.location.hash = "/Deploy";
                    }
                }
                else {
                    window.app.showError(results.Message);
                }
            }
        });


        return false;
    },
    back: function () {
        this.configItemView.saveData(this.currentApp);
        if (this.currentApp == 0)
            window.location.hash = "/AzureConfig";
        else {
            this.currentApp--;
            var app = window.selectedApps.models[this.currentApp];
            $('#config-title').html('Configuring ' + app.attributes.Name + ":");
            $('#config-help-text').html(app.attributes.ConfigHelpText);
            console.log('fuck off!!!');

            this.renderItem(app);
        }

        return false;
    }



});