import { Routes } from '@/@types/routes'
import { lazy } from 'react'

// Prefix path is included in Views.tsx
const adminRoutes: Routes = [
    {
        key: 'reservations',
        path: 'reservations',
        component: lazy(
            () => import('../../views/admin/reservations/ReservationList')
        ),
        authority: [],
        meta: {
            footer: false,
        },
    }, 
]

export default adminRoutes
