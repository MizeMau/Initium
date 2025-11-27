<template>
  <div class="navbar d-flex flex-column shadow position-fixed top-0 start-0 h-100 border-end z-3"
       v-on:mouseenter="test"
       v-on:mouseleave="test">
    <ul class="nav flex-column w-100">
      <li class="nav-item">
        <router-link to="/" 
                     class="nav-link d-flex align-items-center gap-2"
                     v-bind:class="$route.path === '/' ? '' : 'text-body'">
          <i class="bi bi-house fs-4"></i>
          <span v-if="isExpanded" class="text-nowrap">Home</span>
        </router-link>
      </li>

      <li class="nav-item">
        <router-link to="/project-management" 
                     class="nav-link d-flex align-items-center gap-2"
                     v-bind:class="$route.path === '/project-management' ? '' : 'text-body'">
          <i class="bi bi-box fs-4"></i>
          <span v-if="isExpanded" class="text-nowrap">Projects</span>
        </router-link>
      </li>
    </ul>

    <ul class="nav flex-column mt-auto w-100">
      <li class="nav-item">
        <button @click="logout"
                class="nav-link text-body d-flex align-items-center gap-2 bg-transparent border-0">
          <i class="bi bi-box-arrow-right fs-4"></i>
          <span v-if="isExpanded" class="text-nowrap">Logout</span>
        </button>
      </li>
    </ul>
  </div>
</template>

<script>
  import { useUserStore } from '@/stores/authStore'

  export default {
    name: 'NavbarPage',
    data() {
      return {
        userStore: useUserStore(),
        isExpanded: false,
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
      test() {
        this.isExpanded = !this.isExpanded
      },
    },
  }
</script>

<style scoped>
  .navbar {
    width: 60px;
    transition: width 0.3s ease;
    overflow: hidden;
    background-color: var(--bs-body-bg)
  }
    .navbar:hover {
      width: 180px;
    }
</style>