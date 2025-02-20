import { TableQueries } from '../common'
import { ExternalAPIsEnum } from './importData'
import { PaginatedResponse } from './response'

export enum BoardTypeEnum {
    UD,
    RR,
    BB,
    HB,
    FB,
    AI,
    UAI,
    FBPlus,
    AILight,
    HBFlexi,
}

export enum RoomStatusEnum {
    Undefined,
    Available,
    UnAvailable,
    OnRequest,
}

export enum ReservationStatusEnum {
    None,
    Open,
    ClientContacted,
    OnlinePaymentPending,
    OnlinePaymentPartiallyPaid,
    OnlinePaymentFullyPaid,
    ReservedInAgency,
}

export interface FilterParameters {
    activeApiAtTimeOfReservation: ExternalAPIsEnum | null
    reservationStatus: ReservationStatusEnum | null
    checkInStart: string | null
    checkInEnd: string | null
    checkOutStart: string | null
    checkOutEnd: string | null
    createdStart: string | null
    createdEnd: string | null
}

export interface AdultPassengerInfo {
    firstName: string
    lastName: string
}

export interface ChildrenPassengerInfo {
    firstName: string
    lastName: string
    birthDate: string
}

//
// ---- apiGetReservations

export type GetReservationListWithUserAndPaymentQueryParams = TableQueries & {
    // filters
}

export interface ReservationListWithUserAndPaymentUserDto {
    id: number
    firstName: string
    lastName: string
    email: string
    phoneNumber: string
}

export interface ReservationListWithUserAndPaymentVm {
    id: number
    date: string
    reservationStatus: ReservationStatusEnum
    message: string | null
    checkInDate: string
    checkOutDate: string
    adultsNumber: number
    childrenNumber: number
    childrenAges: number[] | null
    adultsInfo: AdultPassengerInfo[]
    childrenInfo: ChildrenPassengerInfo[]
    roomType: string
    boardFinal: string
    status: RoomStatusEnum
    price: number
    priceWithDiscount: number | null
    paymentTerms: string | null
    cancellationFees: string | null
    specialOffers: string | null
    finalPriceInMKDForPayment: number
    markup: number | null
    activeApisAtTimeOfReservation: ExternalAPIsEnum[] // Same as ReservationExternalApiEnum which is added because I was unable to move it in domain on backend
    archivedHotelId: number | null
    archivedHotelName: string
    archivedOfferId: string
    user: ReservationListWithUserAndPaymentUserDto
    agentId: number | null
    isEmailToAgencyDelivered: boolean
    isEmailToClientDelivered: boolean
}

export interface GetReservationListWithUserAndPaymentQueryResponseData {
    reservations: ReservationListWithUserAndPaymentVm[]
}

export type GetReservationListWithUserAndPaymentQueryResponse =
    PaginatedResponse<GetReservationListWithUserAndPaymentQueryResponseData>

//
// ---- apiChangeReservationStatus

export interface ChangeReservationStatusCommand {
    id: number
    status: ReservationStatusEnum
}

//
// ---- apiChangeReservationAgent
export interface ChangeReservationAgentCommand {
    id: number
    agentId: number
}
