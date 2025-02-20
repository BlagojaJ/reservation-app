import { HttpStatusCode } from 'axios'

export enum ResponseStatus {
    Success,
    Fail,
    Error,
}

export interface BaseResponse {
    status: ResponseStatus
    code: HttpStatusCode
}

export interface SuccessResponse<T> extends BaseResponse {
    data: T
}

export interface ErrorResponseMessage {
    title: string
    message: string
}

export interface ErrorResponse extends BaseResponse {
    error: ErrorResponseMessage
}

export interface PaginationMetadata {
    currentPage: number
    pageSize: number
    totalPages: number
    totalItems: number
}

export interface PaginatedResponse<T> extends SuccessResponse<T> {
    metadata: PaginationMetadata
}
