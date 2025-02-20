import { ReservationStatusEnum } from '@/@types/api/reservation'

export default function getReservationStatusEnumColor(
    externalApiEnum: ReservationStatusEnum
) {
    switch (externalApiEnum) {
        case ReservationStatusEnum.Open:
            return 'red'
        case ReservationStatusEnum.ClientContacted:
            return 'orange'
        case ReservationStatusEnum.OnlinePaymentPending:
            return 'yellow'
        case ReservationStatusEnum.OnlinePaymentPartiallyPaid:
            return 'lime'
        case ReservationStatusEnum.OnlinePaymentFullyPaid:
            return 'green'
        case ReservationStatusEnum.ReservedInAgency:
            return 'green'
    }

    return 'gray'
}
