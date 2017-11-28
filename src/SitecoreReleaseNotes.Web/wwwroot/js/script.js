window.onpopstate = function (event) {
    vm.query = event.state.query;
    vm.results = event.state.results;
};

var router = new VueRouter({
    mode: "history",
    routes: []
});

var vm = new Vue({
    router,
    el: "#app",
    data: {
        searching: false,
        query: "",
        results: []
    },
    mounted: function () {
        if (this.$route.query.q) {
            this.query = this.$route.query.q;
            this.search();
        }
    },
    methods: {
        search() {
            if (!this.query)
                return;

            this.searching = true;
            this.$http.get("/api/search?query=" + this.query).then(response => {
                vm.searching = false;
                response.json().then(json => {
                    vm.results = json;

                    var state = {
                        results: this.results,
                        query: this.query
                    };

                    history.pushState(state, null, "?q=" + this.query);
                });
            });
        }
    }
});