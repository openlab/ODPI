window.OdpiApp = Backbone.Model.extend({
    urlRoot: "/App/Available",
    initialize: function (atts, options) {
        this.set({ 'id': atts.Id });
    }
});

window.OdpiAppCollection = Backbone.Collection.extend({
    model: OdpiApp,
    url: "/App/Available",
    initialize: function() {
        console.log("Initializing the collection");
    }
});

window.SelectedAppCollection = Backbone.Collection.extend({
    model: OdpiApp,
    initialize: function() {
        console.log("Initializing the collection");
    }
});