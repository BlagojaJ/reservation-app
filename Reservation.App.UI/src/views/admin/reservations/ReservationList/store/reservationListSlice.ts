import { HttpStatusCode } from 'axios'
import { createAsyncThunk, createSlice, isAnyOf } from '@reduxjs/toolkit'

import {
    ChangeReservationAgentCommand,
    ChangeReservationStatusCommand,
    FilterParameters,
    GetReservationListWithUserAndPaymentQueryParams,
    ReservationListWithUserAndPaymentVm,
} from '@/@types/api/reservation'
import {
    CreatePaymentForReservationCommand,
    GetPaymentsForReservationQuery,
    PaymentForReservationVm,
} from '@/@types/api/reservationPayment'
import { TableQueries } from '@/@types/common'

import {
    apiChangeReservationAgent,
    apiChangeReservationStatus,
    apiGetReservations,
} from '@/services/ReservationService'
import {
    apiCreteReservationPayment,
    apiGetReservationPayments,
} from '@/services/ReservationPaymentService'
import { AgentListVm } from '@/@types/api/agent'
import { apiGetAgents } from '@/services/AgentService'

export const SLICE_NAME = 'reservationList'

export const getReservations = createAsyncThunk(
    SLICE_NAME + '/getReservations',
    async (params: GetReservationListWithUserAndPaymentQueryParams) => {
        const response = await apiGetReservations(params)

        return response.data
    }
)

export const updateReservationStatus = createAsyncThunk(
    SLICE_NAME + '/updateReservationStatus',
    async (params: ChangeReservationStatusCommand) => {
        const response = await apiChangeReservationStatus(params)

        return response.status === HttpStatusCode.NoContent
    }
)

export const updateReservationAgent = createAsyncThunk(
    SLICE_NAME + '/updateReservationAgent',
    async (command: ChangeReservationAgentCommand) => {
        const response = await apiChangeReservationAgent(command)

        return response.status === HttpStatusCode.NoContent
    }
)

export const getReservationPayments = createAsyncThunk(
    SLICE_NAME + '/getReservationPayments',
    async (params: GetPaymentsForReservationQuery) => {
        const response = await apiGetReservationPayments(params)

        return response.data
    }
)

export const createReservationPayment = createAsyncThunk(
    SLICE_NAME + '/createReservationPayment',
    async (command: CreatePaymentForReservationCommand) => {
        const response = await apiCreteReservationPayment(command)

        return response.status === HttpStatusCode.NoContent
    }
)

export const getAgents = createAsyncThunk(
    SLICE_NAME + '/getAgents',
    async () => {
        const response = await apiGetAgents()

        return response.data
    }
)

type PaymentDrawerType = {
    loading: boolean
    error: boolean
    selectedReservation?: ReservationListWithUserAndPaymentVm
    paymentsList: PaymentForReservationVm[]
    paymentsListTotalAmount: number
    addingPayment: boolean
}

export type ReservationListState = {
    loading: boolean
    reservationList: ReservationListWithUserAndPaymentVm[]
    tableData: TableQueries
    filterData: FilterParameters
    updatingReservationStatusIds: number[]
    deleteConfirmation: boolean
    selectedReservation: number
    paymentsDrawer: PaymentDrawerType
    agents: AgentListVm[]
    updatingAgentIds: number[]
}

export const initialTableData: TableQueries = {
    pageNumber: 1,
    pageSize: 10,
    totalItems: 0,
    queryProperty: undefined,
    query: undefined,
    sortBy: undefined,
    sortOrder: undefined,
}

const initialFilterData: FilterParameters = {
    activeApiAtTimeOfReservation: 0,
    reservationStatus: 0,
    checkInStart: null,
    checkInEnd: null,
    checkOutStart: null,
    checkOutEnd: null,
    createdStart: null,
    createdEnd: null,
}

const initialPaymentDrawerData: PaymentDrawerType = {
    loading: false,
    error: false,
    selectedReservation: undefined,
    paymentsList: [],
    paymentsListTotalAmount: 0,
    addingPayment: false,
}

const initialState: ReservationListState = {
    loading: false,
    reservationList: [],
    tableData: initialTableData,
    filterData: initialFilterData,
    updatingReservationStatusIds: [],
    deleteConfirmation: false,
    selectedReservation: 0,
    paymentsDrawer: initialPaymentDrawerData,
    agents: [],
    updatingAgentIds: [],
}

const reservationListSlice = createSlice({
    name: `${SLICE_NAME}/state`,
    initialState,
    reducers: {
        setTableData: (state, action) => {
            state.tableData = action.payload
        },
        resetFilterData: (state) => {
            state.filterData = initialFilterData
        },
        setFilterData: (state, action) => {
            state.filterData = action.payload
        },
        toggleDeleteConfirmation: (state, action) => {
            state.deleteConfirmation = action.payload
        },
        setSelectedReservation: (state, action) => {
            state.selectedReservation = action.payload as number
        },
        updateReservationStatusWithoutThunk: (
            state,
            action: { payload: { id: number; reservationStatus: number } }
        ) => {
            const { id, reservationStatus } = action.payload

            const reservation = state.reservationList.find(
                (res) => res.id === id
            )
            if (reservation) {
                reservation.reservationStatus = reservationStatus
            }
        },
        setSelectedDrawerReservation: (state, action) => {
            state.paymentsDrawer.selectedReservation = action.payload
        },
        resetDrawerState: (state) => {
            state.paymentsDrawer = initialPaymentDrawerData
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(getReservations.fulfilled, (state, action) => {
                state.reservationList = action.payload.data.reservations
                state.tableData.totalItems = action.payload.metadata.totalItems
                state.loading = false
            })

            // ---
            // updateReservationStatus
            .addCase(updateReservationStatus.pending, (state, action) => {
                state.updatingReservationStatusIds.push(action.meta.arg.id)
            })
            .addCase(updateReservationStatus.fulfilled, (state, action) => {
                state.updatingReservationStatusIds =
                    state.updatingReservationStatusIds.filter(
                        (id) => id !== action.meta.arg.id
                    )

                if (action.payload) {
                    const { id, status } = action.meta.arg
                    const reservation = state.reservationList.find(
                        (res) => res.id === id
                    )
                    if (reservation) {
                        reservation.reservationStatus = status
                    }
                }
            })
            .addCase(updateReservationStatus.rejected, (state, action) => {
                state.updatingReservationStatusIds =
                    state.updatingReservationStatusIds.filter(
                        (id) => id !== action.meta.arg.id
                    )
            })

            // ---
            // updateReservationAgent
            .addCase(updateReservationAgent.pending, (state, action) => {
                state.updatingAgentIds.push(action.meta.arg.id)
            })
            .addCase(updateReservationAgent.fulfilled, (state, action) => {
                state.updatingAgentIds = state.updatingAgentIds.filter(
                    (id) => id !== action.meta.arg.id
                )

                if (action.payload) {
                    const { id, agentId } = action.meta.arg
                    const reservation = state.reservationList.find(
                        (res) => res.id === id
                    )
                    if (reservation) {
                        reservation.agentId = agentId
                    }
                }
            })
            .addCase(updateReservationAgent.rejected, (state, action) => {
                state.updatingAgentIds = state.updatingAgentIds.filter(
                    (id) => id !== action.meta.arg.id
                )
            })

            // ---
            // getReservationPayments
            .addCase(getReservationPayments.pending, (state) => {
                state.paymentsDrawer.loading = true
                state.paymentsDrawer.error = false
            })
            .addCase(getReservationPayments.fulfilled, (state, action) => {
                state.paymentsDrawer.loading = false
                state.paymentsDrawer.paymentsList = action.payload.data.payments
                state.paymentsDrawer.paymentsListTotalAmount =
                    action.payload.data.totalAmountOfPayments
            })
            .addCase(getReservationPayments.rejected, (state) => {
                state.paymentsDrawer.loading = false
                state.paymentsDrawer.error = true
            })

            // ---
            // createReservationPayment
            .addCase(createReservationPayment.pending, (state) => {
                state.paymentsDrawer.addingPayment = true
            })
            .addCase(createReservationPayment.fulfilled, (state) => {
                state.paymentsDrawer.addingPayment = false
            })
            .addCase(createReservationPayment.rejected, (state) => {
                state.paymentsDrawer.addingPayment = false
            })

            // ---
            // createReservationPayment
            .addCase(getAgents.fulfilled, (state, action) => {
                state.agents = action.payload.data.agents
            })

            .addMatcher(isAnyOf(getReservations.pending), (state) => {
                state.loading = true
            })
    },
})

export const {
    setTableData,
    setFilterData,
    resetFilterData,
    toggleDeleteConfirmation,
    setSelectedReservation,
    updateReservationStatusWithoutThunk,
    setSelectedDrawerReservation,
    resetDrawerState,
} = reservationListSlice.actions

export default reservationListSlice.reducer
