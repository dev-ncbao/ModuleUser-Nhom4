import { createRouter, createWebHistory } from 'vue-router'

import Login from '@/pages/Login/Login.vue'
import UsersList from '@/pages/UsersList/UsersList.vue'
import UsersAdd from '@/pages/UsersAdd/UsersAdd.vue'
import UsersEdit from '@/pages/UsersEdit/UsersEdit.vue'

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/',
            name: 'Login',
            component: Login,
        },
        {
            path: '/users',
            name: 'UsersList',
            component: UsersList,
        }, 
        {
            path: '/users/add',
            name: 'UsersAdd',
            component: UsersAdd,
        },
        {
            path: '/users/edit/:username',
            name: 'UsersEdit',
            component: UsersEdit,
        }
    ],
})

export default router
