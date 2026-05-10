<template>
    <div class="container vh-100 d-flex align-items-center justify-content-center">
        <div class="border w-100" style="border-radius: 1rem;">
            <div class="row">
                <div class="col-md-6 border-end text-center p-5">
                    <h1 class="fw-bold text-start">Jokes</h1>
                    <div v-if="jokes.jokes.length === 1">
                        <div v-if="jokes.jokes[0].type === 'single'">
                            <div>{{ jokes.jokes[0].joke }}</div>
                        </div>
                        <div v-else>
                            <div class="mb-2">{{ jokes.jokes[0].setup }}</div>
                            <hr />
                            <div class="mt-2">{{ jokes.jokes[0].delivery }}</div>
                        </div>
                    </div>
                    <ul v-else class="list-group">
                        <li v-for="joke in jokes.jokes"
                            :key="joke.id"
                            class="list-group-item mt-2">
                            <div v-if="joke.type === 'single'">
                                <div>{{ joke.joke }}</div>
                            </div>
                            <div v-else>
                                <div class="mb-2">{{ joke.setup }}</div>
                                <hr />
                                <div class="mt-2">{{ joke.delivery }}</div>
                            </div>
                        </li>
                    </ul>
                </div>
                <div class="col-md-6">
                    <div class="card-body p-4 p-lg-5">
                        <form @submit.prevent>
                            <h1 class="fw-bold">Login</h1>
                            <h5 class="mb-3 pb-3">Sign into your account</h5>
                            <div class="mb-4">
                                <label class="form-label">Username:</label>
                                <input v-model="userName"
                                       class="form-control form-control-lg"
                                       type="text"
                                       required />
                            </div>
                            <div class="mb-4">
                                <label class="form-label">Password:</label>
                                <input v-model="password"
                                       class="form-control form-control-lg"
                                       type="password"
                                       required />
                            </div>
                            <div class="pt-2">
                                <button :disabled="isLoading"
                                        class="btn btn-primary btn-lg"
                                        @click="login">
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

<script setup lang="ts">
    import { ref, reactive, onMounted } from 'vue'
    import { useRouter } from 'vue-router'
    import { useUserStore } from '@/stores/authStore'
    import UtilJokeService from '@/service/util/joke'

    interface SingleJoke {
        id: number
        type: 'single'
        joke: string
    }

    interface TwoPartJoke {
        id: number
        type: 'twopart'
        setup: string
        delivery: string
    }

    type Joke = SingleJoke | TwoPartJoke

    interface JokesResponse {
        jokes: Joke[]
    }

    const router = useRouter()
    const userStore = useUserStore()
    const utilJokeService = new UtilJokeService()

    const jokes = reactive < JokesResponse > ({ jokes: [] })
    const userName = ref < string > ('')
    const password = ref < string > ('')
    const isLoading = ref < boolean > (false)
    const error = ref < string | null > (null)

    onMounted(async () => {
        const amount = 2
        if (amount === 1) {
            const joke = await utilJokeService.getAny(amount)
            jokes.jokes = [joke]
        } else {
            const response = await utilJokeService.getAny(amount)
            jokes.jokes = response.jokes
        }
    })

    async function login(): Promise<void> {
        isLoading.value = true
        error.value = null
        try {
            await userStore.login(userName.value, password.value)
            router.push('/')
        } catch (err) {
            console.error(err)
            error.value = 'Invalid username or password.'
        } finally {
            isLoading.value = false
        }
    }
</script>