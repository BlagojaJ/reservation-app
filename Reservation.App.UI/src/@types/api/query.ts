export interface PaginationParameters {
    pageNumber: number
    pageSize: number
}

export interface SearchParameters {
    queryProperty?: string
    query?: string
}

export enum SortOrder {
    asc,
    desc,
}

export interface SortParameters {
    sortBy?: string
    sortOrder?: SortOrder
}
