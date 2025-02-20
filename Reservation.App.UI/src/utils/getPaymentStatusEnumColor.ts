import { PaymentStatusEnum } from '@/@types/api/reservationPayment'

export default function getPaymentStatusEnumColor(
    externalApiEnum: PaymentStatusEnum
) {
    switch (externalApiEnum) {
        case PaymentStatusEnum.Pending:
            return 'yellow'
        case PaymentStatusEnum.Failed:
            return 'red'
        case PaymentStatusEnum.Completed:
            return 'green'
    }

    return 'gray'
}
