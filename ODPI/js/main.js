var AppRouter = Backbone.Router.extend({

    routes: {
        "/AzureConfig": "azure",
        "/Config": "conf",
        "/Deploy": "deploy",
        "*actions": "defaultRoute"
    },

    initialize: function () {

        window.available = new OdpiAppCollection();
    },
    defaultRoute: function () {
        this.hideError();
        console.log("Showing the main Page");

        this.ticketListView = new TaskListView();
        window.available.fetch();
    },
    azure: function () {
        this.hideError();
        console.log("Showing Azure Config Page");
        if (window.azureConfig == null)
            window.azureConfig = new AzureConfig();

        this.azureConfigView = new AzureConfigView({ model: window.azureConfig });
        this.azureConfigView.render();
    },
    conf: function () {
        this.hideError();
        this.confView = new ConfigView();
        this.confView.render();
        $('#content').html(this.confView.el);
    },
    deploy: function () {
        this.hideError();
        this.deployView = new DeployView();
        this.deployView.render();
        $('#content').html(this.deployView.el);
    },
    showError: function (msg) {
        this.errorView = new ErrorView({ model: { message: msg} });
        this.errorView.render();
        $('#error-holder').html(this.errorView.el);
        $('#error-holder').show();
    },
    hideError: function () {
        $('#error-message').hide();
    }

});

tpl.loadTemplates(['AvailAppItem', 'AzureConfig', 'Config', 'ConfigItem', 'Deploy', 'DeployItem', 'Error', 'ManualItem', 'Tasklist'],
    function () {
        tpl.loadConfigTemplates(['SqlAzure', 'OpenIntel', 'BlobStorage', 'OpenTurf', 'DataLab', 'CitizenPortalOpenData'], function () { });
        window.app = new AppRouter();
        Backbone.history.start();
    });
