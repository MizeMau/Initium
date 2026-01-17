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
            children: [
                {
                    path: 'home',
                    name: 'project-management-home',
                    component: () => import('@/components/project-management/home/index.vue'),
                },
                {
                    path: 'project/:id',
                    name: 'project-management-project',
                    component: () => import('@/components/project-management/project/index.vue'),
                },
            ]
        },
        {
            path: '/login',
            name: 'login',
            component: () => import('@/components/login/index.vue'),
        },
    ],
})

router.beforeEach(async (to, from, next) => {
    if (to.fullPath !== '/login') {
        const userStore = useUserStore()
        userStore.getCurrentUser()
    }
    next()
})

export default router
