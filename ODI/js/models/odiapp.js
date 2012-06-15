window.OdiApp = Backbone.Model.extend({
    urlRoot: "/App/Available",
    initialize: function (atts, options) {
        this.set({ 'id': atts.Id });
    }
});

window.OdiAppCollection = Backbone.Collection.extend({
    model: OdiApp,
    url: "/App/Available",
    initialize: function() {
        console.log("Initializing the collection");
    }
});

window.SelectedAppCollection = Backbone.Collection.extend({
    model: OdiApp,
    initialize: function() {
        console.log("Initializing the collection");
    }
});