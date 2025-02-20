import { ReactNode } from 'react'

import { ReservationListWithUserAndPaymentVm } from '@/@types/api/reservation'

import { Button, Tooltip } from '@/components/ui'

import {
    MdOutlineDiscount,
    MdOutlinePayments,
    MdOutlineCancel,
} from 'react-icons/md'

import { useAppDispatch, setSelectedDrawerReservation } from '../store'
import formatToCurrency from '@/utils/formatToCurrency'

const TooltipContent = ({
    title,
    message,
}: {
    title: string
    message: string | ReactNode
}) => {
    return (
        <>
            <div className="font-semibold mb-1">{title}</div>
            <div className="ml-3">{message}</div>
        </>
    )
}

const ReservationTablePriceAndInfoColumn = ({
    reservation,
}: {
    reservation: ReservationListWithUserAndPaymentVm
}) => {
    const dispatch = useAppDispatch()

    const reservationCopy = { ...reservation }
    const priceClasses = 'text-base font-semibold tracking-wide'
    const discountClasses = 'line-through tracking-wide font-medium'

    // NOTE: XXX case: there are special offers, but no original price before discount
    if (
        reservationCopy.specialOffers &&
        reservationCopy.specialOffers.length > 0 &&
        !reservationCopy.priceWithDiscount
    ) {
        reservationCopy.priceWithDiscount = reservationCopy.price
        reservationCopy.price = 0
    }

    const paymentsButtonClickHandler = () => {
        dispatch(setSelectedDrawerReservation(reservation))
    }

    return (
        <div className="flex gap-4 items-center justify-between">
            <div className="text-center leading-4 text-gray-600">
                <div
                    className={
                        reservationCopy.priceWithDiscount
                            ? discountClasses
                            : priceClasses
                    }
                >
                    {reservationCopy.price !== 0
                        ? formatToCurrency(reservationCopy.price, 'EUR', 0)
                        : '/'}
                </div>

                {reservationCopy.priceWithDiscount && (
                    <div className={priceClasses}>
                        {formatToCurrency(
                            reservationCopy.priceWithDiscount,
                            'EUR',
                            0
                        )}
                    </div>
                )}

                <Button
                    size="xs"
                    className=" mt-2"
                    onClick={paymentsButtonClickHandler}
                >
                    Плаќања
                </Button>
            </div>

            <div className="flex flex-col gap-2">
                {reservationCopy.specialOffers && (
                    <Tooltip
                        title={
                            <TooltipContent
                                title="Попуст"
                                message={reservationCopy.specialOffers}
                            />
                        }
                        placement="right"
                        className="text-xs"
                    >
                        <MdOutlineDiscount />
                    </Tooltip>
                )}

                {reservationCopy.paymentTerms && (
                    <Tooltip
                        title={
                            <TooltipContent
                                title="Услови за плаќање"
                                message={reservationCopy.paymentTerms
                                    .split('\r\n')
                                    .map((line, index) => (
                                        <div key={index}>{line}</div>
                                    ))}
                            />
                        }
                        placement="right"
                        className="text-xs"
                    >
                        <MdOutlinePayments />
                    </Tooltip>
                )}

                {reservationCopy.cancellationFees && (
                    <Tooltip
                        title={
                            <TooltipContent
                                title="Такса за откажување"
                                message={reservationCopy.cancellationFees
                                    .split('\r\n')
                                    .map((line, index) => (
                                        <div key={index}>{line}</div>
                                    ))}
                            />
                        }
                        placement="right"
                        className="text-xs"
                    >
                        <MdOutlineCancel />
                    </Tooltip>
                )}
            </div>
        </div>
    )
}

export default ReservationTablePriceAndInfoColumn
