<template>
  <div>
    <navbar-component v-if="this.userStore.user"/>
    <div style="margin-left: 60px">
      <router-view />
    </div>
  </div>
</template>

<script>
  import { useUserStore } from '@/stores/authStore'

  import NavbarComponent from './navbar.vue'

  export default {
    name: 'AppPage',
    components: {
      'navbar-component': NavbarComponent
    },
    data() {
      return {
        userStore: useUserStore(),
      }
    },
  }
</script>