import { PaymentStatusEnum } from '@/@types/api/reservationPayment'

export default function getPaymentStatusEnumName(
    externalApiEnum: PaymentStatusEnum
) {
    switch (externalApiEnum) {
        case PaymentStatusEnum.Pending:
            return 'Во тек'
        case PaymentStatusEnum.Failed:
            return 'Неуспешно'
        case PaymentStatusEnum.Completed:
            return 'Успешно'
    }

    return ''
}
