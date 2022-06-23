<template>
  <div v-show="show" class="flex flex-expand">
    <template v-for="(bot, index) in bots">
      <bot-tile v-bind:data="bot" v-bind:order="index"></bot-tile>
    </template>
  </div>
</template>

<script>
module.exports = {
  name: "bot-overview",
  data: function () {
    return {}
  },
  props: ["show", "bots"],
  mounted() {
  },
  methods: {},
  components: {
    'bot-tile': Vue.defineAsyncComponent(() => loadModule('/vue/objects/bot-tile.vue', window.options)),
  }
}
</script>