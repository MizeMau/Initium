// /src/stores/user.js
import { ref } from 'vue'
import { defineStore } from 'pinia'
import router from '@/router'
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
            const hashedPassword = await hashPassword(password)
            const result = await api.login(username, hashedPassword)
            user.value = result.data || result
        } catch (err) {
            console.error('Login failed:', err)
            error.value = err
            throw err
        } finally {
            loading.value = false
        }
    }

    async function hashPassword(password) {
        const encoder = new TextEncoder()
        const data = encoder.encode(password)
        const hashBuffer = await crypto.subtle.digest('SHA-256', data)
        const hashArray = Array.from(new Uint8Array(hashBuffer))
        const hashHex = hashArray.map(b => b.toString(16).padStart(2, '0')).join('')
        return hashHex
    }

    async function logout() {
        console.log('logout')
        loading.value = true
        error.value = null
        try {
            const api = new userService()
            const result = await api.logout()
            user.value = null
        } catch (err) {
            console.error('logout failed:', err)
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
            router.push('/login')
        }
    }

    return { user, loading, error, login, logout, getCurrentUser }
})
