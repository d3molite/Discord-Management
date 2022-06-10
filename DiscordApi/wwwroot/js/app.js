var vue_data = {
    bots: []
}

var app = Vue.createApp({
    data() {
        return vue_data;
    },
    created() {

    },
    mounted() {
        console.log("Mounted Vue App.");
        this.emitter.on('*', (type, e) => console.log(type, e))
        this.emitter.on('loggedin', (e) => this.getData())
    },
    methods: {
    },
    components: {
    },
});

const emitter = mitt()
app.config.globalProperties.emitter = emitter

var app = app.mount("#app");