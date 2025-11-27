import { createRouter, createWebHistory } from 'vue-router'
import { useUserStore } from '@/stores/authStore'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/',
            name: 'home',
            component: () => import('@/components/home/index.vue'),
        },
        {
            path: '/project-management',
            name: 'project-management',
            component: () => import('@/components/project-management/index.vue'),
        },
        {
            path: '/login',
            name: 'login',
            component: () => import('@/components/login/index.vue'),
        },
    ],
})

router.beforeEach(async (to, from, next) => {
    console.log('Navigating from', from.fullPath, 'to', to.fullPath)
    if (to.fullPath !== '/login') {
        const userStore = useUserStore()
        await userStore.getCurrentUser()
    }
    next() // must call next() to continue navigation
})

export default router
