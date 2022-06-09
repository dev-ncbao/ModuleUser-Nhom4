import { createRouter, createWebHistory } from 'vue-router'

import Hello from '@/pages/hello/hello.vue'

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/',
            name: 'home',
            component: Hello,
        },
    ],
})

export default router
