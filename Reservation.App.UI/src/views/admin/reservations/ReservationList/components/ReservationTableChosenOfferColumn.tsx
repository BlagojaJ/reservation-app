import {
    ReservationListWithUserAndPaymentVm,
    RoomStatusEnum,
} from '@/@types/api/reservation'

import { Badge } from '@/components/ui'

const ReservationTableChosenOfferColumn = ({
    reservation,
}: {
    reservation: ReservationListWithUserAndPaymentVm
}) => {
    let badgeLabel = ''
    let badgeColor = ''

    switch (reservation.status) {
        case RoomStatusEnum.Available:
            badgeLabel = 'ДОСТАПНО'
            badgeColor = 'bg-sky-500'
            break
        case RoomStatusEnum.UnAvailable:
            badgeLabel = 'НЕДОСТАПНО'
            badgeColor = 'bg-red-500'
            break
        case RoomStatusEnum.OnRequest:
            badgeLabel = 'НА БАРАЊЕ'
            badgeColor = 'bg-yellow-500'
            break
    }

    return (
        <div className="flex">
            <div>
                <div className="font-semibold">{reservation.roomType}</div>

                <div>{reservation.boardFinal}</div>

                <Badge innerClass={badgeColor} content={badgeLabel} />
            </div>
        </div>
    )
}

export default ReservationTableChosenOfferColumn
