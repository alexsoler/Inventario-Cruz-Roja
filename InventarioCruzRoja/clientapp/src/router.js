import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

export const router = new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
      component: () => import('@/views/dashboard/Index'),
      children: [
        // Dashboard
        {
          name: 'Dashboard',
          path: '',
          component: () => import('@/views/dashboard/Dashboard'),
        },
        // Pages
        {
          name: 'User Profile',
          path: '/pages/user',
          component: () => import('@/views/dashboard/pages/UserProfile'),
        },
        {
          name: 'Users',
          path: '/users',
          component: () => import('@/views/dashboard/pages/Users'),
        },
        {
          name: 'Roles',
          path: '/roles',
          component: () => import('@/views/dashboard/pages/Roles'),
        },
        {
          name: 'Fabricantes',
          path: '/fabricantes',
          component: () => import('@/views/dashboard/pages/Fabricantes'),
        },
        {
          name: 'Categorias',
          path: '/categorias',
          component: () => import('@/views/dashboard/pages/Categorias'),
        },
        {
          name: 'Sedes',
          path: '/sedes',
          component: () => import('@/views/dashboard/pages/Sedes'),
        },
        {
          name: 'Productos',
          path: '/productos',
          component: () => import('@/views/dashboard/pages/Productos'),
        },
        {
          name: 'Proveedores',
          path: '/proveedores',
          component: () => import('@/views/dashboard/pages/Proveedores'),
        },
        {
          name: 'AgregarProducto',
          path: '/productos/agregar',
          component: () => import('@/views/dashboard/pages/Producto'),
        },
        {
          name: 'EditarProducto',
          path: '/productos/editar/:id',
          component: () => import('@/views/dashboard/pages/Producto'),
          props: true,
        },
        {
          name: 'Ingresos',
          path: '/ingresos',
          component: () => import('@/views/dashboard/pages/Ingresos'),
        },
        {
          name: 'Egresos',
          path: '/Egresos',
          component: () => import('@/views/dashboard/pages/Egresos'),
        },
        {
          name: 'Notifications',
          path: '/components/notifications',
          component: () => import('@/views/dashboard/component/Notifications'),
        },
        {
          name: 'Icons',
          path: '/components/icons',
          component: () => import('@/views/dashboard/component/Icons'),
        },
        {
          name: 'Typography',
          path: '/components/typography',
          component: () => import('@/views/dashboard/component/Typography'),
        },
        // Tables
        {
          name: 'Regular Tables',
          path: 'tables/regular-tables',
          component: () => import('@/views/dashboard/tables/RegularTables'),
        },
        // Maps
        {
          name: 'Google Maps',
          path: 'maps/google-maps',
          component: () => import('@/views/dashboard/maps/GoogleMaps'),
        },
        // Upgrade
        {
          name: 'Upgrade',
          path: 'upgrade',
          component: () => import('@/views/dashboard/Upgrade'),
        },
      ],
    },
    {
      name: 'Login',
      path: '/login',
      component: () => import('@/views/auth/Login'),
    },
  ],
})

router.beforeEach((to, from, next) => {
  const publicPages = ['/login', '/register']
  const authRequired = !publicPages.includes(to.path)
  const loggedIn = localStorage.getItem('user')

  // trying to access a restricted page + not logged in
  // redirect to login page
  if (authRequired && !loggedIn) {
    next('/login')
  } else {
    next()
  }
})
