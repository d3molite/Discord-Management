<template>
  <div class="flex flex-expand">
    <bot-overview v-bind:bots="bots" v-bind:show="tab === 'bot'"></bot-overview>
  </div>
</template>

<script>
module.exports = {
  name: "paginator",
  data: function () {
    return {
      bots: []
    }
  },
  props: ["tab"],
  mounted() {
    this.emitter.on("toggle-tab", (e) => this.toggle(e))
  },
  methods: {
    toggle(data) {
      switch (data.tab) {
        case "bot":
          window.loadBots().then(p => this.bots = p.data);
          console.log(this.bots)
      }
    }
  },
  components: {
    'bot-overview': Vue.defineAsyncComponent(() => loadModule('/vue/pages/bot-overview.vue', window.options)),
  }
}
</script>