// /src/stores/user.js
import { ref } from 'vue'
import { defineStore } from 'pinia'
import userService from '@/service/auth/user'

export const useUserStore = defineStore('user', () => {
    const user = ref(null)
    const loading = ref(false)
    const error = ref(null)

    async function login(username, password) {
        loading.value = true
        error.value = null
        try {
            const api = new userService()
            const result = await api.login(username, password)
            user.value = result.data || result
        } catch (err) {
            console.error('Login failed:', err)
            error.value = err
            throw err
        } finally {
            loading.value = false
        }
    }

    async function logout() {
        loading.value = true
        error.value = null
        try {
            const api = new userService()
            const result = await api.logout()
            user.value = {}
        } catch (err) {
            console.error('Login failed:', err)
            error.value = err
        } finally {
            loading.value = false
        }
    }

    async function getCurrentUser() {
        // optional: if you want to auto-fetch current user on load
        try {
            const api = new userService()
            const result = await api.getCurrentUser?.()
            user.value = result?.data || result
        } catch (err) {
            console.warn('Failed to fetch user:', err)
        }
    }

    return { user, loading, error, login, logout, getCurrentUser }
})
