import {
    ReservationListWithUserAndPaymentVm,
    ReservationStatusEnum,
} from '@/@types/api/reservation'

import { Button, Dropdown } from '@/components/ui'

import getReservationStatusEnumColor from '@/utils/getReservationStatusEnumColor'
import getReservationStatusEnumName from '@/utils/getReservationStatusEnumName'
import {
    updateReservationAgent,
    updateReservationStatus,
    useAppDispatch,
    useAppSelector,
} from '../store'

type ColumnProps = {
    row: ReservationListWithUserAndPaymentVm
}

// TODO: Refactor (could extract common logic)

const ReservationTableStatusDropdown = ({ row }: ColumnProps) => {
    const allowedReservationStatuses = [
        ReservationStatusEnum.Open,
        ReservationStatusEnum.ClientContacted,
        ReservationStatusEnum.ReservedInAgency,
    ]

    const paymentReservationStatuses = [
        ReservationStatusEnum.OnlinePaymentPending,
        ReservationStatusEnum.OnlinePaymentPartiallyPaid,
        ReservationStatusEnum.OnlinePaymentFullyPaid,
    ]

    const updatingReservationStatusIds = useAppSelector(
        (state) => state.reservationList.data.updatingReservationStatusIds
    )

    const dispatch = useAppDispatch()

    const toggleButtonLabel = getReservationStatusEnumName(
        row.reservationStatus
    )
    const toggleButtonColor = getReservationStatusEnumColor(
        row.reservationStatus
    )

    const isUpdating = updatingReservationStatusIds.includes(row.id)
    const isPaymentInProcess = paymentReservationStatuses.includes(
        row.reservationStatus
    )

    const button = (
        <Button
            variant="solid"
            shape="circle"
            size="xs"
            color={`${toggleButtonColor}-500`}
            loading={isUpdating}
            disabled={isPaymentInProcess}
        >
            {toggleButtonLabel}
        </Button>
    )

    const updateReservationStatusHandler = async (
        reservationStatus: ReservationStatusEnum
    ) => {
        await dispatch(
            updateReservationStatus({
                id: row.id,
                status: reservationStatus,
            })
        )
    }

    return (
        <div className="flex justify-center text-lg mb-1">
            <Dropdown
                placement="bottom-center"
                // Note: payment status is not included in the dropdown items hence the activeKey is set to undefined
                activeKey={
                    isPaymentInProcess
                        ? undefined
                        : row.reservationStatus.toString()
                }
                renderTitle={button}
                disabled={isUpdating || isPaymentInProcess}
                menuClass="text-xs"
            >
                {allowedReservationStatuses.map((allowedReservationStatus) => {
                    const dropdownItemLabel = getReservationStatusEnumName(
                        allowedReservationStatus
                    )

                    return (
                        <Dropdown.Item
                            key={allowedReservationStatus}
                            eventKey={allowedReservationStatus.toString()}
                            onSelect={(eventKey) =>
                                updateReservationStatusHandler(Number(eventKey))
                            }
                        >
                            {dropdownItemLabel}
                        </Dropdown.Item>
                    )
                })}
            </Dropdown>
        </div>
    )
}

const ReservationTableAgentDropdown = ({ row }: ColumnProps) => {
    const agents = useAppSelector((state) => state.reservationList.data.agents)
    const updatingAgentIds = useAppSelector(
        (state) => state.reservationList.data.updatingAgentIds
    )

    const dispatch = useAppDispatch()

    let toggleButtonLabel = ''
    if (!row.agentId) {
        toggleButtonLabel = 'Нема агент'
    } else {
        toggleButtonLabel =
            agents.find((agent) => agent.id === row.agentId)?.name ?? 'Грешка'
    }

    const isUpdating = updatingAgentIds.includes(row.id)

    const button = (
        <Button
            shape="circle"
            size="xs"
            color={`gray-500`}
            loading={isUpdating}
            disabled={isUpdating}
        >
            {toggleButtonLabel}
        </Button>
    )

    const updateReservationAgentHandler = async (agentId: number) => {
        await dispatch(
            updateReservationAgent({
                id: row.id,
                agentId: agentId,
            })
        )
    }

    return (
        <div className="flex justify-center text-lg">
            <Dropdown
                placement="bottom-center"
                // Note: payment status is not included in the dropdown items hence the activeKey is set to undefined
                activeKey={row.agentId ? row.agentId.toString() : undefined}
                renderTitle={button}
                disabled={isUpdating}
                menuClass="text-xs"
            >
                {agents.map((agent) => {
                    return (
                        <Dropdown.Item
                            key={agent.id}
                            eventKey={agent.id.toString()}
                            onSelect={(eventKey) =>
                                updateReservationAgentHandler(Number(eventKey))
                            }
                        >
                            {agent.name}
                        </Dropdown.Item>
                    )
                })}
            </Dropdown>
        </div>
    )
}

const ReservationTableStatusAndAgentColumn = ({ row }: ColumnProps) => {
    return (
        <>
            <ReservationTableStatusDropdown row={row} />
            <ReservationTableAgentDropdown row={row} />
        </>
    )
}

export default ReservationTableStatusAndAgentColumn
