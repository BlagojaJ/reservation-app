import { useEffect } from 'react'
import reducer, { getAgents, useAppDispatch, SLICE_NAME } from './store'
import { injectReducer } from '@/store'
import AdaptableCard from '@/components/shared/AdaptableCard'
// import ReservationTableTools from './Components/ReservationTableTools'
import ReservationTable from './components/ReservationTable'
import ExternalApiBadgeNameLegend from '@/components/shared/custom/ExternalApiBadgeNameLegend'
import ReservationTableTools from './components/ReservationTableTools'

injectReducer(SLICE_NAME, reducer)

const ReservationList = () => {
    const dispatch = useAppDispatch()

    useEffect(() => {
        dispatch(getAgents())
    }, [dispatch])

    return (
        <AdaptableCard className="h-full" bodyClass="h-full">
            <div className="items-center justify-between mb-4 lg:flex">
                <h3 className="mb-4 lg:mb-0">Резервации</h3>
                <ExternalApiBadgeNameLegend disableTooltip />
                <ReservationTableTools />
            </div>
            <ReservationTable />
        </AdaptableCard>
    )
}

export default ReservationList
