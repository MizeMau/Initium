import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'

import * as echarts from 'echarts'

import { useUserStore } from '@/stores/authStore'

import 'bootstrap-icons/font/bootstrap-icons.css'

import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap/dist/js/bootstrap.bundle.js'
import './styles/amethyst-dark.css'
import './styles/bootstrap-addon.css'

const app = createApp(App)
const pinia = createPinia()

app.use(pinia)
app.use(router)

app.config.globalProperties.$echarts = echarts
app.provide('echarts', echarts)

const userStore = useUserStore()
await userStore.getCurrentUser()

app.mount('#app')
