<template>
  <div>
    <div>
      <button class="logout-btn"
              @click="logout">
        <i class="bi bi-box-arrow-right"></i>
      </button>
    </div>
    <router-view />
  </div>
</template>

<script>import { useUserStore } from '@/stores/authStore'

  export default {
    name: 'AppPage',
    data() {
      return {
        userStore: useUserStore(),
      }
    },
    methods: {
      async logout() {
        try {
          await this.userStore.logout()
          this.$router.push('/login')
        } catch (err) {
        }
      },
    },
  }
</script>

<style scoped>
  .logout-btn {
    position: absolute;
    right: 10px;
    top: 10px;
    background: #3a3a3a;
    color: #fbc531;
    font-size: 2rem;
    border-radius: 50%;
    width: 60px;
    height: 60px;
    cursor: pointer;
    padding-left: 10px;
  }
</style>