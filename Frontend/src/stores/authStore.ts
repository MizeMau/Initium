import { ref } from 'vue'
import { defineStore } from 'pinia'
import router from '@/router'
import UserService from '@/service/auth/user'
import type { User } from '@/service/auth/user'

export const useUserStore = defineStore('user', () => {
    const user = ref<User | null>(null)
    const loading = ref(false)
    const error = ref<Error | null>(null)

    async function login(username: string, password: string): Promise<void> {
        loading.value = true
        error.value = null
        try {
            const api = new UserService()
            const hashedPassword = await hashPassword(password)
            const result = await api.login(username, hashedPassword)
            user.value = result
        } catch (err) {
            console.error('Login failed:', err)
            error.value = err as Error
            throw err
        } finally {
            loading.value = false
        }
    }

    async function hashPassword(password: string): Promise<string> {
        const encoder = new TextEncoder()
        const data = encoder.encode(password)
        const hashBuffer = await crypto.subtle.digest('SHA-256', data)
        return Array.from(new Uint8Array(hashBuffer))
            .map(b => b.toString(16).padStart(2, '0'))
            .join('')
    }

    async function logout(): Promise<void> {
        loading.value = true
        error.value = null
        try {
            const api = new UserService()
            await api.logout()
            user.value = null
        } catch (err) {
            console.error('Logout failed:', err)
            error.value = err as Error
        } finally {
            loading.value = false
        }
    }

    async function getCurrentUser(): Promise<void> {
        try {
            const api = new UserService()
            const result = await api.getCurrentUser?.()
            user.value = result
        } catch (err) {
            console.warn('Failed to fetch user:', err)
            router.push('/login')
        }
    }

    return { user, loading, error, login, logout, getCurrentUser }
})
