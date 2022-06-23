<template>
  <div v-show="opened" class="main-app">
    <slot name="nav">

    </slot>
    <slot name="main">

    </slot>
  </div>
</template>

<script>
module.exports = {
  name: "main-app",
  data: function () {
    return {
      opened: false,
    }
  },
  props: [],
  created() {
    this.emitter.on('toggle:main-app', (e) => this.toggle())
  },
  mounted() {

  },
  methods: {
    toggle() {
      this.opened = !this.opened;
    },
  },
}
</script>