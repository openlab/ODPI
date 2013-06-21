window.DeployItemView = Backbone.View.extend({
    tagName: "div",

    events: {
        'click #btndeploy': 'build'
    },

    initialize: function () {
        this.template = _.template(tpl.get('DeployItem'));
        this.model.bind("change", this.render, this);
        this.model.bind("destroy", this.close, this);
    },

    render: function (eventName) {
        $(this.el).html(this.template(this.model.toJSON()));
        var serviceSelect = $(this.el).find('#service-select');

        _.each(window.azureConfig.services, function (serv) {
            serviceSelect.append("<option>" + serv + "</option>");
        }, this);
        return this;
    },

    build: function () {
        $(this.el).find('#btndeploy').hide(); //.attr('disabled', 'disabled');
        $(this.el).find('#service-selector').hide();
        $(this.el).find('#status-div').show();
        $(this.el).find('.alert').addClass('alert-info');
        var service = $(this.el).find('#service-select').val();
        var self = this;
        $.ajax({
            type: 'POST',
            url: '/Deploy/Build',
            data:
                {
                    'key': window.azureConfig.key,
                    'service': service,
                    'data': JSON.stringify(self.model.settings[0].toJSON())
                },
            success: function (results) {
                if (results.Status == 0) {
                    self.handleMessage(results);
                    self.upload();
                }
                else {
                    self.handleError(results);
                }
            }
        });
        return false;
    },

    upload: function () {
        var self = this;
        var ac = window.azureConfig;
        var azure = { 'subscriptionId': ac.subscriptionId, 'storageName': ac.storageName, 'storageKey': ac.storageKey, 'key': ac.key };
        $.ajax({
            type: 'POST',
            url: '/Deploy/Upload',
            data:
                {
                    'azure': JSON.stringify(azure),
                    'data': JSON.stringify(self.model.settings[0].toJSON())
                },
            success: function (results) {
                if (results.Status == 0) {
                    self.file = results.Data;
                    self.handleMessage(results);
                    self.deploy();
                }
                else {
                    self.handleError(results);
                }
            }
        });
    },

    deploy: function () {
        var self = this;
        var ac = window.azureConfig;
        self.service = $(self.el).find('#service-select').val();
        $.ajax({
            type: 'POST',
            url: '/Deploy/Deploy',
            data:
                {
                    'azure': JSON.stringify({
                        'subscriptionId': ac.subscriptionId,
                        'storageName': ac.storageName,
                        'storageKey': ac.storageKey,
                        'key': ac.key,
                        'file': self.file,
                        'service': $(self.el).find('#service-select').val()
                    }),
                    'data': JSON.stringify(self.model.settings[0].toJSON())

                },
            success: function (results) {
                if (results.Status == 0) {
                    self.handleMessage(results);
                    self.token = results.Data;
                    self.status();
                }
                else {
                    self.handleError(results);
                }

            }
        });
    },
    statusUrl: '/Deploy/Status',
    status: function () {
        var self = this;
        var ac = window.azureConfig;
        var azure = { 'subscriptionId': ac.subscriptionId, 'storageName': ac.storageName, 'storageKey': ac.storageKey, 'key': ac.key };
        var token = self.token;
        if (self.statusUrl == "/Deploy/DeployStatus")
            token = self.service;
        $.ajax({
            type: 'POST',
            url: self.statusUrl,
            data:
                {
                    'azure': JSON.stringify(azure),
                    'token': token
                },
            success: function (results) {
                self.handleMessage(results);
                if (results.Status == 2) { //Inprogress
                    setTimeout(function () { self.status() }, 5000);
                }
                else if (results.Status == 0) {
                    if (self.statusUrl == '/Deploy/Status') {
                        self.statusUrl = '/Deploy/DeployStatus'
                        setTimeout(function () { self.status() }, 5000);
                    }
                    else {
                        self.postDeploy();
                    }
                }
                else //Error
                    self.handleError(results);
            }
        });
    },

    postDeploy: function () {
        var self = this;
        var ac = window.azureConfig;
        var azure = { 'subscriptionId': ac.subscriptionId, 'storageName': ac.storageName, 'storageKey': ac.storageKey, 'key': ac.key };
        $.ajax({
            type: 'POST',
            url: '/Deploy/PostDeploy',
            data:
                {
                    'azure': JSON.stringify(azure),
                    'data': JSON.stringify(self.model.settings[0].toJSON()),
                    'service': self.service
                },
            success: function (results) {
                if (results.Status == 0) {
                    console.log(results);
                    $(self.el).find('.alert').removeClass('alert-info');
                    $(self.el).find('.alert').addClass('alert-success');
                    $(self.el).find('#status-div').hide();
                    $(self.el).find('#success').show();
                    //TODO: extract the actual url from the results
                    $(self.el).find('#url').html("<a href='" + results.Data + "' target='_blank'>ici</a>");
                }
                else {
                    self.handleError(results);
                }
            }
        });
    },

    handleMessage: function (message) {
        console.log(message.Status);
        console.log(message.LogMessage);
        $(this.el).find('#current-step').html(message.Stage);
        $(this.el).find('.collapse').prepend(message.LogMessage + "<br />");
    },

    handleError: function (message) {
        console.log(message);
        $(this.el).find('#current-step').html(message.Stage);
        $(this.el).find('.collapse').prepend(message.LogMessage + "<br />");
        $(this.el).find('.alert').removeClass('alert-info');
        $(this.el).find('.alert').addClass('alert-error');
    }
});