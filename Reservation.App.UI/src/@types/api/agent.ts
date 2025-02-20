import { SuccessResponse } from './response'

//
// ---- apiGetAgents
export interface AgentListVm {
    id: number
    name: string
}

export interface GetAgentListQueryResponseData {
    agents: AgentListVm[]
}
export type GetAgentListQueryResponse =
    SuccessResponse<GetAgentListQueryResponseData>

//
// ---- apiCreateAgent
export interface CreateAgentCommand {
    name: string
}
