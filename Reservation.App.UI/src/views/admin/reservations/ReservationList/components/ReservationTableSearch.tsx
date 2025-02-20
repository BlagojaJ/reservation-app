import { useEffect, useRef } from 'react'
import Input from '@/components/ui/Input'
import { HiOutlineSearch } from 'react-icons/hi'
import {
    setTableData,
    useAppSelector,
    useAppDispatch,
    getReservations,
} from '../store'
import debounce from 'lodash/debounce'
import cloneDeep from 'lodash/cloneDeep'
import type { TableQueries } from '@/@types/common'
import type { ChangeEvent } from 'react'

const CLIENT_SEARCH = 'User_FirstName_LastName'
const HOTEL_SEARCH = 'ArchivedHotelName'

const ReservationTableSearch = () => {
    const dispatch = useAppDispatch()

    const clientSearchInput = useRef<HTMLInputElement>(null)
    const hotelSearchInput = useRef<HTMLInputElement>(null)

    const tableData = useAppSelector(
        (state) => state.reservationList.data.tableData
    )
    const filterData = useAppSelector(
        (state) => state.reservationList.data.filterData
    )

    const debounceFn = debounce(handleDebounceFn, 500)

    function handleDebounceFn(prop: string, val: string) {
        const newTableData = cloneDeep(tableData)
        newTableData.queryProperty = prop
        newTableData.query = val
        newTableData.pageNumber = 1

        if (prop === CLIENT_SEARCH) {
            if (hotelSearchInput.current) {
                hotelSearchInput.current.value = ''
            }
        }

        if (prop === HOTEL_SEARCH) {
            if (clientSearchInput.current) {
                clientSearchInput.current.value = ''
            }
        }

        if (typeof val === 'string' && val.length > 1) {
            fetchData(newTableData)
        }

        if (typeof val === 'string' && val.length === 0) {
            fetchData(newTableData)
        }
    }

    const fetchData = (data: TableQueries) => {
        dispatch(setTableData(data))
        dispatch(getReservations({ ...data, ...filterData }))
    }

    const onEdit = (prop: string, e: ChangeEvent<HTMLInputElement>) => {
        debounceFn(prop, e.target.value)
    }

    useEffect(() => {
        const updateInputValue = (
            inputRef: React.RefObject<HTMLInputElement>,
            queryProperty: string,
            query: string | undefined
        ) => {
            if (inputRef.current) {
                inputRef.current.value =
                    tableData.queryProperty === queryProperty ? query ?? '' : ''
            }
        }

        updateInputValue(clientSearchInput, CLIENT_SEARCH, tableData.query)
        updateInputValue(hotelSearchInput, HOTEL_SEARCH, tableData.query)
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])

    return (
        <>
            <Input
                ref={clientSearchInput}
                className="w-full lg:w-[182px]"
                size="sm"
                placeholder="Пребарај по клиент"
                prefix={<HiOutlineSearch className="text-lg" />}
                onChange={(e) => onEdit(CLIENT_SEARCH, e)}
            />
            <Input
                ref={hotelSearchInput}
                className="w-full lg:w-[182px]"
                size="sm"
                placeholder="Пребарај по хотел"
                prefix={<HiOutlineSearch className="text-lg" />}
                onChange={(e) => onEdit(HOTEL_SEARCH, e)}
            />
        </>
    )
}

export default ReservationTableSearch
