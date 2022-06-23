var vue_data = {
    token: "",
    user: "",
    tab: "bot",
    bots: []
}

var app = Vue.createApp({
    data() {
        return vue_data;
    },
    created() {
    },
    update() {
    },
    mounted() {
        console.log("Mounted Vue App.");
        this.emitter.on('*', (type, e) => console.log(type, e))
        this.emitter.on('logged-in', (e) => this.initialLoad(e))
        this.emitter.on('logout', (e) => this.logout())
    },
    methods: {
        refresh() {
            this.$nextTick(() => {
                let token = window.localStorage.getItem("token");
                if (token !== undefined && token !== null) {
                    window.verify(token).then(
                        p => {
                            setTimeout(() => {
                                this.user = p.data.user;
                                this.emitter.emit("toggle:login-window");
                                this.emitter.emit("toggle:main-app");
                                this.emitter.emit("toggle-tab", {tab: "bot"})
                            }, 50);

                        }
                    ).catch();
                }
            })
        },
        logout() {
            let token = window.localStorage.getItem("token")
            if (token !== undefined && token !== null) {
                window.logout(token).then(
                    p => {
                        this.emitter.emit("toggle:login-window");
                        this.emitter.emit("toggle:main-app");
                    }
                ).catch(
                    p => {
                        this.emitter.emit("toggle:login-window");
                        this.emitter.emit("toggle:main-app");
                    }
                )
            }
        },
        initialLoad(data) {
            this.user = data.username;
            console.log("Initial Load Initialized");
            this.emitter.emit("toggle:login-window");
            this.emitter.emit("toggle:main-app");
        },
    },
    components: {
        'login-window': Vue.defineAsyncComponent(() => loadModule('/vue/login-window.vue', window.options)),
        'main-app': Vue.defineAsyncComponent(() => loadModule('/vue/main-app.vue', window.options)),
        'nav-bar': Vue.defineAsyncComponent(() => loadModule('/vue/nav-bar.vue', window.options)),
        'paginator': Vue.defineAsyncComponent(() => loadModule('/vue/paginator.vue', window.options)),
    },
});

const emitter = mitt()
app.config.globalProperties.emitter = emitter

var app = app.mount("#app");

window.onload = function () {
    app.refresh();
}