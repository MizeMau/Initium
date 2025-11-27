<template>
  <div class="login-page">
    <div class="login-card">
      <h2 class="title">Login</h2>

      <div class="form-group">
        <label for="username">Username</label>
        <input id="username"
               v-model="userName"
               type="text"
               placeholder="Enter username"
               required />
      </div>

      <div class="form-group">
        <label for="password">Password</label>
        <input id="password"
               v-model="password"
               type="password"
               placeholder="Enter password"
               required />
      </div>

      <button @click="login" :disabled="isLoading" class="btn">
        <span v-if="!isLoading">Login</span>
        <span v-else>Logging in...</span>
      </button>

      <p v-if="error" class="error">{{ error }}</p>
    </div>
  </div>
</template>

<script>
  import { useUserStore } from '@/stores/authStore'

  export default {
    name: 'LoginPage',
    data() {
      return {
        userName: '',
        password: '',
        isLoading: false,
        error: null,
        userStore: useUserStore(),
      }
    },
    mounted() {

    },
    beforeDestroy() {
      cancelAnimationFrame(this.animationFrame)
      window.removeEventListener('mousemove', this.onMouseMove)
    },
    methods: {
      async login() {
        this.isLoading = true
        this.error = null
        try {
          await this.userStore.login(this.userName, this.password)
          this.$router.push('/')
        } catch (err) {
          console.error(err)
          this.error = 'Invalid username or password.'
        } finally {
          this.isLoading = false
        }
      },
    },
  }
</script>