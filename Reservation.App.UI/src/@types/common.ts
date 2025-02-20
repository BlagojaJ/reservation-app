import { ReactNode, CSSProperties } from 'react'
import {
    PaginationParameters,
    SearchParameters,
    SortParameters,
} from './api/query'

export interface CommonProps {
    className?: string
    children?: ReactNode
    style?: CSSProperties
}

export type TableQueries = PaginationParameters &
    SearchParameters &
    SortParameters & {
        totalItems?: number
        totalPages?: number
    }

export type Image = {
    id?: number
    name?: string
    img: string
}
