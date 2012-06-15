window.AzureConfigView = Backbone.View.extend({

    initialize: function () {
        this.template = _.template(tpl.get('AzureConfig'));
        this.model.bind("change", this.render, this);
        this.model.bind("destroy", this.close, this);
    },

    events: {
        'click #btn-back': 'back',
        'click #certlink': 'getCert',
        'click #continue_button': 'next',
        'click #skip': 'skip'
    },

    render: function (eventName) {
        console.log(this.model);
        $(this.el).html(this.template());
        $(this.el).find("#subid").val(this.model.subscriptionId);
        $(this.el).find("#saname").val(this.model.storageName);
        $(this.el).find("#sakey").val(this.model.storageKey);

        $('#content').html(this.el);
        if (this.model.key != null)
            $('#continue_button').removeAttr('disabled');

        //enable tooltips
        $("[rel=tooltip]").tooltip();
    },

    back: function () {
        this.saveData();
        window.location.hash = '/';
    },

    getCert: function (event) {
        var self = this;
        $.ajax({
            type: 'POST',
            url: '/Check/Register',
            success: function (results) {
                console.log(results);
                if (results.Status == 0) { //Status = OK
                    self.model.key = results.Data;
                    window.location.href = "/Azure/GenCert?key=" + self.model.key;
                    $('#continue_button').removeAttr('disabled');
                }
            }
        });
        return false;
    },

    saveData: function () {
        this.model.subscriptionId = $(this.el).find("#subid").val();
        this.model.storageName = $(this.el).find("#saname").val();
        this.model.storageKey = $(this.el).find("#sakey").val();
        window.azureConfig = this.model;
        console.log(window.azureConfig);
    },

    next: function () {
        var btnCont = $('#continue_button')
        if (btnCont.attr('disabled') == null) {

            this.saveData();
            var self = this;

            // show loading
            this.disableContinue(true);

            $.ajax({
                type: 'POST',
                url: '/Check/InitAzure',
                data:
                {
                    'id': self.model.key,
                    'subId': self.model.subscriptionId,
                    'name': self.model.storageName,
                    'key': self.model.storageKey
                },
                success: function (results) {
                    self.enableContinue();
                    console.log(results);
                    if (results.Status == 0) { //Status = OK
                        self.model.services = results.Data.Services;
                        window.location.hash = '/Config';
                    }
                    else { //Status = Error
                        app.showError(results.Message);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    self.enableContinue();
                    console.log(textStatus);
                }
            });
        }
        return false;
    },

    enableContinue: function () {
        var btn = $('#continue_button');
        btn.removeAttr('disabled');
        btn.html('Continue');

        btn = $('#skip').removeAttr('disabled');
    },

    disableContinue: function (showLoading) {
        var btn = $('#continue_button');
        btn.attr('disabled', 'disabled');
        if (showLoading) {
            btn.html('<img src="/Content/img/loader.gif" alt="Wait" /> Wait ...');
        }

        btn = $('#skip').attr('disabled', 'disabled');
    },

    skip: function () {
        if ($('#skip').attr('disabled') == null) {
            this.saveData();
            window.azureConfig.manualInstall = true;
            window.location.hash = '/Config';
        }
    }


});