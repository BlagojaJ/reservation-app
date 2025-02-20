import { ReservationStatusEnum } from '@/@types/api/reservation'

export default function getReservationStatusEnumName(
    externalApiEnum: ReservationStatusEnum
) {
    switch (externalApiEnum) {
        case ReservationStatusEnum.None:
            return 'Исклучен'
        case ReservationStatusEnum.Open:
            return 'Отворена'
        case ReservationStatusEnum.ClientContacted:
            return 'Kонтактиран клиент'
        case ReservationStatusEnum.OnlinePaymentPending:
            return 'Наплата во тек'
        case ReservationStatusEnum.OnlinePaymentPartiallyPaid:
            return 'Платена делумно'
        case ReservationStatusEnum.OnlinePaymentFullyPaid:
            return 'Платена'
        case ReservationStatusEnum.ReservedInAgency:
            return 'Резервирано во агенција'
    }

    return ''
}
