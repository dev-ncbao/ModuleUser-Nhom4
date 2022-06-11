import { nextTick } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: () => import('../views/Homeview.vue')
    },
    {
      path: '/user',
      name: 'user',
      component: () => import('../views/UserView.vue')
    },
    {
      path: '/user/add',
      name: 'add',
      component: () => import('../components/UserAdd.vue')
    },
    {
      path: '/user/edit/:username',
      name: 'edit',
      component: () => import('../components/UserEdit.vue')
    },
  ]
})

// router.beforeEach(() => {
//   const key = localStorage.getItem('key')
//   if (key) {
//     router.push('/user') 
//   }
//   return true
// })

export default router
