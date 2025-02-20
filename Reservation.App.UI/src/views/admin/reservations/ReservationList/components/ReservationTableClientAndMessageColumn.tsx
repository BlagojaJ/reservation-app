import { ReservationListWithUserAndPaymentVm } from '@/@types/api/reservation'
import { Tooltip } from '@/components/ui'

import { FiMessageSquare } from 'react-icons/fi'
import { MdAlternateEmail, MdOutlineCall } from 'react-icons/md'
import {
    RiBookmark2Line,
    RiBookmarkLine,
    RiMailCheckLine,
    RiMailCloseLine,
} from 'react-icons/ri'

const ReservationTableClientAndMessageColumn = ({
    reservation,
}: {
    reservation: ReservationListWithUserAndPaymentVm
}) => {
    const {
        user,
        message,
        isEmailToClientDelivered,
        isEmailToAgencyDelivered,
    } = reservation

    return (
        <div className="flex items-center gap-4 justify-between">
            <div>
                <div className="font-semibold">{`${user.firstName} ${user.lastName}`}</div>
                <div className="ml-1">
                    <div className="flex gap-1 items-center">
                        <MdAlternateEmail className="text-blue-500" />
                        {user.email}
                    </div>
                    <div className="flex gap-1 items-center">
                        <MdOutlineCall className="text-blue-500" />
                        {user.phoneNumber}
                    </div>
                </div>
            </div>

            <div className="flex flex-col">
                {message && (
                    <>
                        <Tooltip
                            title={
                                <>
                                    <div className="font-semibold mb-1">
                                        Порака од клиент:
                                    </div>
                                    <div className="ml-3">{message}</div>
                                </>
                            }
                            placement="right"
                            className="text-xs"
                        >
                            <FiMessageSquare />
                        </Tooltip>

                        <hr className="border-t border-gray-300 w-full my-2" />
                    </>
                )}

                {isEmailToClientDelivered ? (
                    <Tooltip
                        title="Потврдата за примено барање за резервација е успешно доставена на е-поштата на клиентот"
                        placement="right"
                        className="text-xs"
                    >
                        <RiMailCheckLine className="text-green-600 mb-1.5" />
                    </Tooltip>
                ) : (
                    <Tooltip
                        title="Потврдата за примено барање за резервација не е доставена на е-поштата на клиентот"
                        placement="right"
                        className="text-xs"
                    >
                        <RiMailCloseLine className="mb-1.5" />
                    </Tooltip>
                )}

                {isEmailToAgencyDelivered ? (
                    <Tooltip
                        title="Барањето за резервација е успешно доставено на е-поштата за резервации"
                        placement="right"
                        className="text-xs"
                    >
                        <RiBookmarkLine className="text-green-600" />
                    </Tooltip>
                ) : (
                    <Tooltip
                        title="Барањето за резервација не е доставено на е-поштата за резервации"
                        placement="right"
                        className="text-xs"
                    >
                        <RiBookmark2Line />
                    </Tooltip>
                )}
            </div>
        </div>
    )
}

export default ReservationTableClientAndMessageColumn
