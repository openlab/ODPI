window.TaskListView = Backbone.View.extend({

    initialize: function () {
        this.template = _.template(tpl.get('Tasklist'));
        window.available.bind('reset', this.render, this);
        
    },
    events: {
        'click #btnContinue': 'nextPage',
        'click #showMore': 'showHideBeforeStart'
    },

    render: function (eventName) {
        $(this.el).html(this.template());
        var self = this;
        _.each(window.available.models, function (app) {
            var v = new AvailAppView({ model: app });
            v.parentView = self;
            $(this.el).find('#app-list').append(v.render().el);
        }, this);

        $('#content').html(this.el);
        this.checkForSelected();

        //hide the help text
        $('#beforeStart').hide();

        //enable tooltips
        $("[rel=tooltip]").tooltip();
    },

    checkEnabled: function () {
        if (window.selectedApps.models.length > 0)
            $("#btnContinue").removeAttr("disabled");
        else
            $("#btnContinue").attr("disabled", "disabled");
    },

    nextPage: function () {
        if ($("#btnContinue").attr("disabled") == null) {
            console.log('Going to Azure Config');
            window.location.hash = '/AzureConfig';
        }
        return false;
    },

    checkForSelected: function () {
        if (window.selectedApps == null) {
            window.selectedApps = new SelectedAppCollection();
        }

        if (window.selectedApps.models.length == 0) {
            return;
        }

        _.each(window.selectedApps.models, function (app) {
            //Select the correct item.
            var div = $('#app-' + app.attributes.Id);
            div.toggleClass('alert-success');
            div.find('.enable').attr('disabled', 'disabled');
            div.find('.disable').removeAttr('disabled');
        });

        this.checkEnabled();
    },

    showHideBeforeStart: function(){
        var panel = $('#beforeStart');
        if(panel.css('display') === 'block'){
            $('#showMore').html('Show More');
            panel.slideUp();          
        }
        else{
            $('#showMore').html('Show Less');
            panel.slideDown();
        }
        return false;
    }

});