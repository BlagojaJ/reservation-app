import { combineReducers } from '@reduxjs/toolkit'
import reducers, {
    ReservationListState,
    SLICE_NAME,
} from './reservationListSlice'
import { useSelector } from 'react-redux'

import type { TypedUseSelectorHook } from 'react-redux'
import type { RootState } from '@/store'

const reducer = combineReducers({
    data: reducers,
})

export const useAppSelector: TypedUseSelectorHook<
    RootState & {
        [SLICE_NAME]: {
            data: ReservationListState
        }
    }
> = useSelector

export * from './reservationListSlice'
export { useAppDispatch } from '@/store'
export default reducer
