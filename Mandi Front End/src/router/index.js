import Vue from 'vue'
import Router from 'vue-router'
import HelloWorld from '@/components/HelloWorld'
import Login from '@/components/Login'

Vue.use(Router)

const router = new Router({
  mode: 'history',
  routes: [
    {
      path: '/',
      name: 'HelloWorld',
      component: HelloWorld
    },
    {
      path: '/login',
      name: 'Login',
      component: Login
    }
  ]
})

router.beforeEach((to, from, next) => {
  console.log(to.name, localStorage.getItem('companyName'))
  if (to.name === 'Login' && localStorage.getItem('userLoggedIn')) {
    return next('/')
  }
  if ((to.name !== 'Login' && from.name !== 'Login') && !localStorage.getItem('userLoggedIn')) {
    return next('/login')
  }
  next()
})

export default router
