import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
        path: '/',
        name: 'home',
        component: () => import('@/components/home/index.vue'),
        },
        {
        path: '/login',
        name: 'login',
        component: () => import('@/components/login/index.vue'),
        },
    ],
})

export default router
