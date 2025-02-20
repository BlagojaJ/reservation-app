import { Button } from '@/components/ui'
import { useAppDispatch } from '@/store'
import { HiOutlineRefresh } from 'react-icons/hi'
import { getReservations, useAppSelector } from '../store'

function ReservationTableRefresh() {
    const dispatch = useAppDispatch()

    const { tableData, filterData } = useAppSelector(
        (state) => state.reservationList.data
    )

    const handleRefresh = () => {
        dispatch(getReservations({ ...tableData, ...filterData }))
    }

    return (
        <Button
            shape="circle"
            size="sm"
            variant="twoTone"
            icon={<HiOutlineRefresh />}
            onClick={handleRefresh}
        />
    )
}

export default ReservationTableRefresh
