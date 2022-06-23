<template>
  <div>
    <div v-show="opened" class="flex flex-expand flex-center">
      <div class="flex animate__animated login-window login-border ui"
           v-bind:class="{animate__shakeX: shake, animate__zoomIn: zoomin} ">
        <div class="flex flex-col flex-expand margin">
          <div class="flex flex-center flex-col login-item">
            <div class="login-title">
              Please log in to continue.
            </div>
            <div v-show="hasError" class="login-error">
              {{ error }}
            </div>
          </div>
          <div class="flex flex-col flex-col-center-h flex-expand flex-space-between">
            <div class="flex flex-center">
              <div class="margin-lr">
                Username:
              </div>
              <input v-model="username"/>
            </div>
            <div class="flex flex-center">
              <div class="margin-lr">
                Password:
              </div>
              <input v-model="password" type="password" v-on:keyup.enter="login()"/>
            </div>
            <div class="button pointer flex flex-center" @click="login()">
              LOGIN
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
module.exports = {
  name: "login-window",
  data: function () {
    return {
      opened: true,
      username: "",
      password: "",
      error: "",
      shake: false,
      zoomin: true,
    }
  },
  created() {
    this.emitter.on("toggle:login-window", (e) => {
      this.toggle();
    });
  },
  mounted() {
  },
  props: [],
  computed: {
    hasError() {
      return this.error.length > 0;
    }
  },
  methods: {
    toggle() {
      this.opened = !this.opened;
      if (this.opened) {
        this.zoomin = true;
        setTimeout(() => this.zoomin = false, 500);
      }
    },
    login() {
      window.login(this.username, this.password)
          .then(p => this.close(this.username))
          .catch(p => {
                this.error = "Username or password were incorrect."
                this.zoomin = false;
                this.shake = true;
                setTimeout(() => this.shake = false, 500);
              }
          )
    },
    close(username) {
      this.error = "";
      this.emitter.emit("logged-in", {"username": username})
      this.opened = false;
    },
  },
}
</script>

<style scoped>
@import '../css/components/login-window.css';
</style>