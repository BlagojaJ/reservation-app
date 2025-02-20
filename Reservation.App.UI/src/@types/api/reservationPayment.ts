//
// ---- apiCreteReservationPayment

import { SuccessResponse } from './response'

export enum PaymentStatusEnum {
    Pending,
    Completed,
    Failed,
}

export interface CreatePaymentForReservationCommand {
    reservationId: number
    amount: number
}

//
// ---- apiGetReservationPayments
export interface GetPaymentsForReservationQuery {
    reservationId: number
}

export interface PaymentForReservationVm {
    id: number
    amount: number
    paymentCreationDate: string
    paymentDate: string | null
    status: PaymentStatusEnum
    gatewayTransactionId: string
}

export interface GetPaymentsForReservationQueryResponseData {
    payments: PaymentForReservationVm[]
    totalAmountOfPayments: number
}

export type GetPaymentsForReservationQueryResponse =
    SuccessResponse<GetPaymentsForReservationQueryResponseData>
