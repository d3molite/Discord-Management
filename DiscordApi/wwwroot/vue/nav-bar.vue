<template>
  <div class="nav-bar flex flex-space-between animate__animated"
       v-bind:class="{animate__fadeInDown: slideIn, animate__fadeOutUp: !slideIn}">
    <div class="flex flex-center h100">
      <div class="nav-button" v-bind:class="{nav_active: selected === 'bot'}">
        <div class="" @click="switchTab">
          <span class="nav-icon material-icons">
            smart_toy
          </span>
        </div>
      </div>
    </div>
    <div v-show="isLoggedIn" class="flex margin-lr">
      <div class="margin-lr">
        Logged in as {{ username }}
      </div>
      <div class="underline pointer" @click="logout()">
        logout
      </div>
    </div>
  </div>
</template>

<script>
module.exports = {
  name: "nav-bar",
  data: function () {
    return {
      opened: false,
      slideIn: true,
      selected: "bot"
    }
  },
  props: ["username", "closed"],
  mounted() {
  },
  methods: {
    logout() {
      this.slideIn = false;
      setTimeout(() => {
        this.emitter.emit("logout")
        this.slideIn = true;
      }, 500);

    },
    switchTab() {
      this.emitter.emit("toggle-tab", {tab: this.selected})
    }
  },
  components: {},
  computed: {
    isLoggedIn() {
      return this.username.length > 0
    }
  }
}
</script>