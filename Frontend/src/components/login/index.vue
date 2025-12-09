<template>
  <div class="container vh-100 d-flex align-items-center justify-content-center">
    <div class="border w-100"
         style="border-radius: 1rem;">
      <div class="row">
        <div class="col-md-6 border-end text-center p-5">
          <h1 class="fw-bold text-start">Jokes</h1>
          <div v-if="jokes.jokes.length === 1">
            <div v-if="jokes.jokes[0].type === 'single'">
              <div>{{jokes.jokes[0].joke}}</div>
            </div>
            <div v-else>
              <div class="mb-2">{{jokes.jokes[0].setup}}</div>
              <hr />
              <div class="mt-2">{{jokes.jokes[0].delivery}}</div>
            </div>
          </div>
          <ul v-else
              v-for="joke in jokes.jokes"
              class="list-group">
            <li class="list-group-item mt-2">
              <div v-if="joke.type === 'single'">
                <div>{{joke.joke}}</div>
              </div>
              <div v-else>
                <div class="mb-2">{{joke.setup}}</div>
                <hr />
                <div class="mt-2">{{joke.delivery}}</div>
              </div>
            </li>
          </ul>
        </div>
        <div class="col-md-6">
          <div class="card-body p-4 p-lg-5">
            <form>
              <h1 class="fw-bold">Login</h1>
              <h5 class="mb-3 pb-3">
                Sign into your account
              </h5>
              <div class="mb-4">
                <label class="form-label">Username:</label>
                <input class="form-control form-control-lg"
                       v-model="userName"
                       type="text"
                       required />
              </div>
              <div class="mb-4">
                <label class="form-label">Password:</label>
                <input class="form-control form-control-lg"
                       v-model="password"
                       type="password"
                       required />
              </div>
              <div class="pt-2">

                <button v-on:click="login" 
                        v-bind:disabled="isLoading" 
                        class="btn btn-primary btn-lg">
                  <span v-if="!isLoading">Login</span>
                  <span v-else>Logging in...</span>
                </button>
              </div>
              <p v-if="error" class="error">{{ error }}</p>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
  import { useUserStore } from '@/stores/authStore'

  import UtilJokeService from "@/service/util/joke"

  export default {
    data() {
      return {
        userStore: useUserStore(),
        utilJokeService: new UtilJokeService(),
        jokes: {
          jokes: [],
        },
        userName: '',
        password: '',
        isLoading: false,
        error: null,
      }
    },
    async mounted() {
      const amount = 2;
      if (amount == 1) {
        const joke = await this.utilJokeService.getAny(amount)
        this.jokes.jokes = [joke]
      }
      else {
        this.jokes = await this.utilJokeService.getAny(amount)
      }
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