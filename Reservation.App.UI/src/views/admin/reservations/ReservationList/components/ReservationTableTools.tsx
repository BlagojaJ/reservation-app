import ReservationTableSearch from './ReservationTableSearch'
import ReservationFilter from './ReservationFilter'
import ReservationTableRefresh from './ReservationTableRefresh'

const ReservationTableTools = () => {
    return (
        <div className="flex flex-col gap-2 lg:flex-row lg:items-center">
            <ReservationTableSearch />
            <ReservationFilter />
            <ReservationTableRefresh />
        </div>
    )
}

export default ReservationTableTools
