import { api4 } from './../api'
import { createRouter, createWebHistory } from 'vue-router'
import axios from 'axios'

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

router.beforeEach(async (to, from) => {
  // if (to.path === '/')
  //   return '/user'

  const key = localStorage.getItem('username')
  let redir = null
  if (key) {
    await axios.get(`${api4}/${key}`)
      .then(res => {
        if (to.path === '/') {
          redir = '/user'
        }
        else redir = true
      })
      .catch(err => {
        if (to.path !== '/') {
          redir = '/'
        }
        else redir = true
      })
    return redir
  }
  else {
    if (to.path !== '/') {
      return '/'
    }
    else return true
  }
})

export default router
