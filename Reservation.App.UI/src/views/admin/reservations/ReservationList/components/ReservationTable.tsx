import { useEffect, useMemo, useRef } from 'react'

import { SortParameters } from '@/@types/api/query'
import { ReservationListWithUserAndPaymentVm } from '@/@types/api/reservation'
import type {
    DataTableResetHandle,
    ColumnDef,
} from '@/components/shared/DataTable'

import DataTable from '@/components/shared/DataTable'
import ReservationTableClientAndMessageColumn from './ReservationTableClientAndMessageColumn'
import ReservationTableQueryColumn from './ReservationTableQueryColumn'
import ReservationTableChosenOfferColumn from './ReservationTableChosenOfferColumn'
import ReservationTablePriceAndInfoColumn from './ReservationTablePriceAndInfoColumn'
import ReservationTableExternalApiColumn from './ReservationTableExternalApiColumn'
import ReservationTableStatusAndAgentColumn from './ReservationTableStatusAndAgentColumn'

import {
    getReservations,
    setTableData,
    useAppDispatch,
    useAppSelector,
} from '../store'

import cloneDeep from 'lodash/cloneDeep'
import formatDateString from '@/utils/formatDate'

import { IoIosWarning } from 'react-icons/io'
import PaymentsDrawer from './PaymentsDrawer'

const ReservationTable = () => {
    const tableRef = useRef<DataTableResetHandle>(null)

    const dispatch = useAppDispatch()

    const {
        pageNumber,
        pageSize,
        totalItems,
        queryProperty,
        query,
        sortBy,
        sortOrder,
    } = useAppSelector((state) => state.reservationList.data.tableData)

    const filterData = useAppSelector(
        (state) => state.reservationList.data.filterData
    )

    const loading = useAppSelector(
        (state) => state.reservationList.data.loading
    )

    const data = useAppSelector(
        (state) => state.reservationList.data.reservationList
    )

    useEffect(() => {
        fetchData()
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [pageNumber, pageSize, sortBy, sortOrder])

    useEffect(() => {
        if (tableRef) {
            tableRef.current?.resetSorting()
        }
    }, [filterData])

    const tableData = useMemo(
        () => ({
            pageNumber,
            pageSize,
            sortBy,
            sortOrder,
            totalItems,
        }),
        [pageNumber, pageSize, sortBy, sortOrder, totalItems]
    )

    const fetchData = () => {
        dispatch(
            getReservations({
                pageNumber,
                pageSize,
                queryProperty,
                query,
                sortBy,
                sortOrder,
                ...filterData,
            })
        )
    }

    const columns: ColumnDef<ReservationListWithUserAndPaymentVm>[] = useMemo(
        () => [
            {
                header: 'ИД/ДАТА',
                id: 'id',
                cell: (props) => {
                    const reservation = props.row.original

                    return (
                        <div className="text-center">
                            <div className="text-xs">{reservation.id}</div>
                            <div>{formatDateString(reservation.date)}</div>
                        </div>
                    )
                },
            },
            {
                header: 'Клиент',
                // accessorKey: 'user.firstName',
                cell: (props) => (
                    <ReservationTableClientAndMessageColumn
                        reservation={props.row.original}
                    />
                ),
            },
            {
                header: 'Пребарување',
                // accessorKey: 'archivedHotelName',
                cell: (props) => (
                    <ReservationTableQueryColumn
                        reservation={props.row.original}
                    />
                ),
            },
            {
                header: 'Избрана понуда',
                // accessorKey: 'roomType',
                cell: (props) => (
                    <ReservationTableChosenOfferColumn
                        reservation={props.row.original}
                    />
                ),
            },
            {
                header: 'Цена',
                // accessorKey: "price",
                cell: (props) => (
                    <ReservationTablePriceAndInfoColumn
                        reservation={props.row.original}
                    />
                ),
            },
            {
                id: 'externalFromApis',
                header: 'API',
                cell: (props) => (
                    <ReservationTableExternalApiColumn
                        apis={
                            props.cell.row.original
                                .activeApisAtTimeOfReservation
                        }
                    />
                ),
            },
            {
                header: 'МП',
                cell: (props) => <div>{props.row.original.markup ?? 0}%</div>,
            },
            {
                header: '',
                id: 'status',
                cell: (props) => (
                    <ReservationTableStatusAndAgentColumn
                        row={props.row.original}
                    />
                ),
            },
        ],
        []
    )

    const onPaginationChange = (page: number) => {
        const newTableData = cloneDeep(tableData)
        newTableData.pageNumber = page
        dispatch(setTableData(newTableData))
    }

    const onSelectChange = (value: number) => {
        const newTableData = cloneDeep(tableData)
        newTableData.pageSize = Number(value)
        newTableData.pageNumber = 1
        dispatch(setTableData(newTableData))
    }

    const onSort = (sort: SortParameters) => {
        const newTableData = cloneDeep(tableData)
        newTableData.sortBy = sort.sortBy
        newTableData.sortOrder = sort.sortOrder
        dispatch(setTableData(newTableData))
    }

    return (
        <>
            <DataTable
                ref={tableRef}
                columns={columns}
                data={data}
                loading={loading}
                pagingData={{
                    total: tableData.totalItems as number,
                    pageIndex: tableData.pageNumber as number,
                    pageSize: tableData.pageSize as number,
                }}
                onPaginationChange={onPaginationChange}
                onSelectChange={onSelectChange}
                onSort={onSort}
            />

            <hr className="mt-5 mb-7" />

            <div className="flex gap-3 items-center mx-4 my-3">
                <IoIosWarning className="h-9 w-9 text-red-500" />
                <div className="leading-4 text-xs">
                    {/* cSpell: disable */}
                    xxx
                    <br />
                    xxx
                    {/* cSpell: enable */}
                </div>
            </div>

            <PaymentsDrawer />
        </>
    )
}

export default ReservationTable
