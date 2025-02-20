import { ReservationListWithUserAndPaymentVm } from '@/@types/api/reservation'

import { TbArrowBarRight, TbArrowBarToRight } from 'react-icons/tb'
import { BiMaleFemale, BiChild } from 'react-icons/bi'

import formatDateString from '@/utils/formatDate'
import { Tooltip } from '@/components/ui'

const ReservationTableQueryColumn = ({
    reservation,
}: {
    reservation: ReservationListWithUserAndPaymentVm
}) => {
    const reservationAdultsNumber = (
        <div className="flex gap-1 items-center">
            <BiMaleFemale className="text-red-500" />
            {reservation.adultsNumber}
        </div>
    )

    const childrenNumberWithAges = (
        <div className="flex gap-1 items-center">
            <BiChild className="text-red-500 w-4 h-4" />
            {reservation.childrenNumber}

            {reservation.childrenAges != null &&
                reservation.childrenAges.length > 0 && (
                    <span className="ml-2">
                        ({reservation.childrenAges.join(', ')})
                    </span>
                )}
        </div>
    )

    return (
        <>
            <div className=" mb-0.5">{reservation.archivedHotelName}</div>

            <div className="flex items-center space-x-5 ">
                <div className="flex gap-1 items-center">
                    <TbArrowBarToRight className="text-red-500" />
                    {formatDateString(reservation.checkInDate)}
                </div>

                <div className="flex gap-1 items-center">
                    <TbArrowBarRight className="text-red-500" />
                    {formatDateString(reservation.checkOutDate)}
                </div>
            </div>

            <div className="flex items-center space-x-5">
                {reservation.adultsInfo.length > 0 ? (
                    <Tooltip
                        title={reservation.adultsInfo.map((a, index) => (
                            <div key={index}>
                                Возрасен: {a.firstName} {a.lastName}
                            </div>
                        ))}
                        placement="right"
                        className="text-xs"
                    >
                        {reservationAdultsNumber}
                    </Tooltip>
                ) : (
                    reservationAdultsNumber
                )}

                {reservation.childrenInfo.length > 0 ? (
                    <Tooltip
                        title={reservation.childrenInfo.map((a, index) => (
                            <div key={index}>
                                Дете: {a.firstName} {a.lastName} (
                                {formatDateString(a.birthDate)})
                            </div>
                        ))}
                        placement="right"
                        className="text-xs"
                    >
                        {childrenNumberWithAges}
                    </Tooltip>
                ) : (
                    childrenNumberWithAges
                )}
            </div>
        </>
    )
}

export default ReservationTableQueryColumn
