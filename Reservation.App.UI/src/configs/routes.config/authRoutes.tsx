import { lazy } from 'react'
import type { Routes } from '@/@types/routes'
import { AUTH_PREFIX_PATH } from '@/constants/route.constant'

// Include prefix path
const authRoute: Routes = [
    {
        key: 'signIn',
        path: `${AUTH_PREFIX_PATH}/sign-in`,
        component: lazy(() => import('@/views/auth/SignIn')),
        authority: [],
    },
]

export default authRoute
