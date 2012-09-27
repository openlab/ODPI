var AppRouter = Backbone.Router.extend({

    routes: {
        "/AzureConfig": "azure",
        "/Config": "conf",
        "/About": "about",
        "/faq": "faq",
        "/Deploy": "deploy",
        "*actions": "defaultRoute"
    },

    initialize: function () {

        window.available = new OdiAppCollection();

        this.faqView = new FaqView();
        this.aboutView = new AboutView();
    },
    about: function () {
        this.hideError();
        console.log("building about view");
        this.aboutView.render();
        $('#content').html(this.aboutView.el);
    },
    faq: function () {
        this.hideError();
        console.log("building faq view");
        this.faqView.render();
        $('#content').html(this.faqView.el);
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

tpl.loadTemplates(['Tasklist', 'About', 'Faq', 'AvailAppItem', 'AzureConfig', 'Config', 'Deploy', 'DeployItem', 'Error', 'ManualItem'],
    function () {
        tpl.loadConfigTemplates(['SqlAzure', 'OpenIntel', 'BlobStorage', 'OpenTurf', 'DataLab', 'CitizenPortalOpenData'], function () { });
        window.app = new AppRouter();
        Backbone.history.start();
    });


