export type SignInCredential = {
    username: string
    password: string
}

export type SignInResponse = {
    id: string
    userName: string
    email: string
    token: string
}

export interface AuthenticationResponse {
    id: string
    userName: string
    email: string
    token: string
}

export type SignUpResponse = SignInResponse

export type SignUpCredential = {
    userName: string
    email: string
    password: string
}

export type ForgotPassword = {
    email: string
}

export type ResetPassword = {
    password: string
}
