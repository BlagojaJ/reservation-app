import {
    CreatePaymentForReservationCommand,
    GetPaymentsForReservationQuery,
    GetPaymentsForReservationQueryResponse,
} from '@/@types/api/reservationPayment'

import ApiService from './ApiService'

export async function apiGetReservationPayments(
    command: GetPaymentsForReservationQuery
) {
    return ApiService.fetchData<GetPaymentsForReservationQueryResponse>({
        url: `/reservations/${command.reservationId}/payments`,
        method: 'get',
    })
}

export async function apiCreteReservationPayment(
    command: CreatePaymentForReservationCommand
) {
    return ApiService.fetchData({
        url: `/reservations/${command.reservationId}/payments`,
        method: 'post',
        data: command,
    })
}
