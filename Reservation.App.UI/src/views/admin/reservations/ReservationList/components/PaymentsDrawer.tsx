import { ChangeEvent, useEffect, useState } from 'react'

import {
    Alert,
    Badge,
    Button,
    Drawer,
    Input,
    Skeleton,
    Table,
} from '@/components/ui'
import TBody from '@/components/ui/Table/TBody'
import Td from '@/components/ui/Table/Td'
import Th from '@/components/ui/Table/Th'
import THead from '@/components/ui/Table/THead'
import Tr from '@/components/ui/Table/Tr'

import { useAppDispatch } from '@/store'
import {
    createReservationPayment,
    getReservationPayments,
    resetDrawerState,
    setSelectedDrawerReservation,
    updateReservationStatusWithoutThunk,
    useAppSelector,
} from '../store'

import { ReservationListWithUserAndPaymentVm } from '@/@types/api/reservation'
import formatDateStringWithTimezone from '@/utils/formatDateStringWithTimezone'
import formatToCurrency from '@/utils/formatToCurrency'
import getPaymentStatusEnumName from '@/utils/getPaymentStatusEnumName'
import getPaymentStatusEnumColor from '@/utils/getPaymentStatusEnumColor'

import { FiPlus } from 'react-icons/fi'

type PaymentsDrawerTitleProps = {
    reservation: ReservationListWithUserAndPaymentVm
}

const PaymentsDrawerTitle = ({ reservation }: PaymentsDrawerTitleProps) => {
    return (
        <div className="flex gap-24 w-full items-center justify-between my-2 mx-4">
            <h4>Плаќања за резервација #{reservation.id}</h4>

            <div className="flex gap-14 items-center">
                <div className="leading-4">
                    <div className="uppercase tracking-wide">
                        {reservation.archivedHotelName}
                    </div>
                    <div>{reservation.roomType}</div>
                </div>

                <div className="font-semibold mb-1">
                    {`${reservation.user.firstName} ${reservation.user.lastName}`}
                </div>
            </div>
        </div>
    )
}

const SkeletonTableBody = ({ rows = 3, columns = 6 }) => {
    return (
        <>
            {Array.from({ length: rows }).map((_, rowIndex) => (
                <Tr key={rowIndex}>
                    {Array.from({ length: columns }).map((_, colIndex) => (
                        <Td key={colIndex}>
                            <Skeleton />
                        </Td>
                    ))}
                </Tr>
            ))}
        </>
    )
}

const PaymentsDrawerTable = () => {
    const { loading, error, paymentsList } = useAppSelector(
        (state) => state.reservationList.data.paymentsDrawer
    )

    return (
        <div>
            <Table compact>
                <THead>
                    <Tr>
                        <Th>Интерно ИД</Th>
                        <Th>Екстерно ИД</Th>
                        <Th>Дата на креирање</Th>
                        <Th>Дата на плаќање</Th>
                        <Th>Износ</Th>
                        <Th>Статус</Th>
                    </Tr>
                </THead>

                <TBody>
                    {loading && <SkeletonTableBody />}

                    {!loading &&
                        paymentsList.map((p) => {
                            const statusLabel = getPaymentStatusEnumName(
                                p.status
                            )
                            const statusColor = `bg-${getPaymentStatusEnumColor(
                                p.status
                            )}-500`

                            return (
                                <Tr key={p.id}>
                                    <Td>{p.id}</Td>

                                    <Td>{p.gatewayTransactionId || '/'}</Td>

                                    <Td>
                                        {formatDateStringWithTimezone(
                                            p.paymentCreationDate
                                        )}
                                    </Td>

                                    <Td>
                                        {p.paymentDate
                                            ? formatDateStringWithTimezone(
                                                  p.paymentDate
                                              )
                                            : '/'}
                                    </Td>

                                    <Td>{formatToCurrency(p.amount, 'MKD')}</Td>

                                    <Td>
                                        <Badge
                                            content={statusLabel}
                                            className={statusColor}
                                        />
                                    </Td>
                                </Tr>
                            )
                        })}
                </TBody>
            </Table>

            {!loading && !error && paymentsList.length === 0 && (
                <Alert
                    showIcon
                    type="info"
                    title="Нема плаќања"
                    className="m-4"
                >
                    Ново плаќање за оваа резервација може да додадете во истиов
                    прозорец
                </Alert>
            )}

            {error && (
                <Alert showIcon type="danger" title="Проблем" className="m-4">
                    Настана проблем при превземањето на плаќањата за оваа
                    резервација
                </Alert>
            )}
        </div>
    )
}

type PaymentDrawerFooterProps = {
    reservation: ReservationListWithUserAndPaymentVm
}

const PaymentDrawerFooter = ({ reservation }: PaymentDrawerFooterProps) => {
    const [amountForAdding, setAmountForAdding] = useState<string>()

    const dispatch = useAppDispatch()

    const paymentsListTotalAmount = useAppSelector(
        (state) =>
            state.reservationList.data.paymentsDrawer.paymentsListTotalAmount
    )
    const addingPayment = useAppSelector(
        (state) => state.reservationList.data.paymentsDrawer.addingPayment
    )

    const remainingAmount =
        reservation.finalPriceInMKDForPayment -
        paymentsListTotalAmount -
        (amountForAdding ? parseFloat(amountForAdding) : 0)

    const addPaymentHandler = async () => {
        if (reservation && amountForAdding) {
            const success = await dispatch(
                createReservationPayment({
                    reservationId: reservation.id,
                    amount: parseFloat(amountForAdding),
                })
            ).unwrap()

            if (success) {
                setAmountForAdding(undefined)

                dispatch(
                    getReservationPayments({
                        reservationId: reservation.id,
                    })
                )

                dispatch(
                    updateReservationStatusWithoutThunk({
                        id: reservation.id,
                        reservationStatus: 3,
                    })
                )
            }
        }
    }

    const amountForAddingChangeHandler = (e: ChangeEvent<HTMLInputElement>) => {
        setAmountForAdding(e.target.value)
    }

    return (
        <div className="w-full flex justify-between mx-4 mb-1">
            <div>
                <div>
                    Цена:{' '}
                    <span className="font-semibold text-gray-700 tracking-wide">
                        {' '}
                        {formatToCurrency(reservation.price, 'EUR', 0)}
                    </span>
                </div>

                <div>
                    Цена со попуст:{' '}
                    <span className="font-semibold text-gray-700 tracking-wide">
                        {' '}
                        {reservation.priceWithDiscount
                            ? formatToCurrency(
                                  reservation.priceWithDiscount,
                                  'EUR',
                                  0
                              )
                            : '/'}
                    </span>
                </div>
            </div>

            <div>
                <div>
                    Цена за плаќање:{' '}
                    <span className="font-semibold text-gray-700 tracking-wide">
                        {' '}
                        {formatToCurrency(
                            reservation.finalPriceInMKDForPayment,
                            'MKD'
                        )}
                    </span>
                </div>

                <div>
                    Останато:{' '}
                    <span className="font-semibold text-gray-700 tracking-wide">
                        {' '}
                        {formatToCurrency(remainingAmount, 'MKD')}
                    </span>
                </div>
            </div>

            <div className="flex items-center gap-2">
                <Input
                    size="sm"
                    placeholder="Износ во MKD"
                    value={amountForAdding}
                    type="number"
                    onChange={amountForAddingChangeHandler}
                />

                <Button
                    shape="circle"
                    variant="solid"
                    size="xs"
                    className="flex-shrink-0"
                    icon={<FiPlus />}
                    loading={addingPayment}
                    onClick={addPaymentHandler}
                />
            </div>
        </div>
    )
}

const PaymentsDrawer = () => {
    const dispatch = useAppDispatch()
    const selectedReservation = useAppSelector(
        (state) => state.reservationList.data.paymentsDrawer.selectedReservation
    )

    const isOpen = !!selectedReservation

    const onDrawerClose = () => {
        // Note: Not here dispatch(resetDrawerState()) because the content inside the drawer will change before it is hidden
        dispatch(setSelectedDrawerReservation(undefined))
    }

    useEffect(() => {
        // 👉 If there is a selected reservation => get its payments
        if (selectedReservation) {
            dispatch(
                getReservationPayments({
                    reservationId: selectedReservation.id,
                })
            )
        }
        // 👉 If there is NOT a selected reservation => reset the drawer state
        else {
            // Timeout is used to reset the state once the drawer is completely hidden (not to cause changing of the content during the hiding animation)
            setTimeout(() => {
                dispatch(resetDrawerState())
            }, 100)
        }
    }, [dispatch, selectedReservation])

    return (
        <>
            <Drawer
                title={
                    isOpen && (
                        <PaymentsDrawerTitle
                            reservation={selectedReservation}
                        />
                    )
                }
                footer={
                    isOpen && (
                        <PaymentDrawerFooter
                            reservation={selectedReservation}
                        />
                    )
                }
                placement="bottom"
                height={500}
                drawerClass="rounded-t-2xl"
                isOpen={isOpen}
                onClose={onDrawerClose}
                onRequestClose={onDrawerClose}
            >
                <div className="h-full grid sm:grid-cols-1 gap-12 ">
                    <PaymentsDrawerTable />
                </div>
            </Drawer>
        </>
    )
}

PaymentsDrawer.displayName = 'FilterForm'

export default PaymentsDrawer
