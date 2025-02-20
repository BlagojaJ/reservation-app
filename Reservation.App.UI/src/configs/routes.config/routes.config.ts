import authRoutes from './authRoutes'
import type { Routes } from '@/@types/routes'
import adminRoutes from './adminRoutes'

export const publicRoutes: Routes = [...authRoutes]

export const protectedRoutes: Routes = [...adminRoutes]
