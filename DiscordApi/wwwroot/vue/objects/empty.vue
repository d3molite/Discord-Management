<template>
  <div v-show="opened">
  </div>
</template>

<script>
module.exports = {
  name: "empty",
  data: function () {
    return {
      opened: false,
    }
  },
  props: [],
  mounted() {
    this.emitter.on('toggle:empty', (e) => this.toggle())
  },
  methods: {
    toggle() {
      this.opened = !this.opened;
    },
  },
  components: {}
}
</script>