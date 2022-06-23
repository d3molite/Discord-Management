<template>
  <div :style="{'animation-delay': (order+1)/4 + 's'}" class="bot-tile animate__animated animate__fadeIn">
    <div class="flex flex-col flex-expand">
      <div class="bot-tile-title flex flex-row-center-v">
        {{ data.name }}
      </div>
      <div class="flex flex-row flex-expand flex-row-center-v">
        <div class="bot-tile-img-wrapper">
          <img :alt="data.name" :src="data.avatar" class="bot-tile-img"/>
        </div>
        <div class="flex flex-expand flex-col margin-left">
          <div class="bot-title-status">
            {{ data.status }}
          </div>
          <div class="bot-title-status">
            {{ data.servers }}
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
module.exports = {
  name: "bot-tile",
  data: function () {
    return {}
  },
  props: ["data", "order"],
  mounted() {
  },
  methods: {},
  components: {}
}
</script>

<style>
@import "../../css/components/bot-tile.css";
</style>