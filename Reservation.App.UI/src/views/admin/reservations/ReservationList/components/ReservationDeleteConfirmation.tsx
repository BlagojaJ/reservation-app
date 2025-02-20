import toast from '@/components/ui/toast'
import Notification from '@/components/ui/Notification'
import ConfirmDialog from '@/components/shared/ConfirmDialog'
import {
    deleteReservation,
    getReservations,
    toggleDeleteConfirmation,
    useAppDispatch,
    useAppSelector,
} from '../store'

const ReservationDeleteConfirmation = () => {
    const dispatch = useAppDispatch()
    const dialogOpen = useAppSelector(
        (state) => state.reservationList.data.deleteConfirmation
    )
    const selectedReservation = useAppSelector(
        (state) => state.reservationList.data.selectedReservation
    )
    const tableData = useAppSelector(
        (state) => state.reservationList.data.tableData
    )
    const filterData = useAppSelector(
        (state) => state.reservationList.data.filterData
    )

    const onDialogClose = () => {
        dispatch(toggleDeleteConfirmation(false))
    }

    const onDelete = async () => {
        dispatch(toggleDeleteConfirmation(false))

        try {
            await dispatch(
                deleteReservation({ id: selectedReservation })
            ).unwrap()

            dispatch(getReservations({ ...tableData, ...filterData }))

            toast.push(
                <Notification
                    title="Успешно бришење"
                    type="success"
                    duration={2500}
                >
                    Резервацијата е успешно избришана
                </Notification>,
                {
                    placement: 'top-center',
                }
            )
        } catch (error) {
            console.log(error)
        }
    }

    return (
        <ConfirmDialog
            isOpen={dialogOpen}
            type="danger"
            title="Избриши резервација"
            confirmButtonColor="red-600"
            onClose={onDialogClose}
            onRequestClose={onDialogClose}
            onCancel={onDialogClose}
            onConfirm={onDelete}
        >
            <p>
                {/* cSpell: disable */}
                Дали сте сигурни дека сакате да ја избришете оваа резервација?
            </p>
        </ConfirmDialog>
    )
}

export default ReservationDeleteConfirmation
