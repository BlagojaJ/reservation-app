import {
    ChangeReservationAgentCommand,
    ChangeReservationStatusCommand,
    GetReservationListWithUserAndPaymentQueryParams,
    GetReservationListWithUserAndPaymentQueryResponse,
} from '@/@types/api/reservation'
import ApiService from './ApiService'

export async function apiGetReservations(
    params: GetReservationListWithUserAndPaymentQueryParams
) {
    return ApiService.fetchData<GetReservationListWithUserAndPaymentQueryResponse>(
        {
            url: '/reservations',
            method: 'get',
            params,
        }
    )
}

export async function apiChangeReservationStatus(
    command: ChangeReservationStatusCommand
) {
    return ApiService.fetchData({
        url: `/reservations/${command.id}/status`,
        method: 'post',
        data: command,
    })
}

export async function apiChangeReservationAgent(
    command: ChangeReservationAgentCommand
) {
    return ApiService.fetchData({
        url: `/reservations/${command.id}/agent`,
        method: 'post',
        data: command,
    })
}
