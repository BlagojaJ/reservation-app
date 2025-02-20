import axios from 'axios'
import appConfig from '@/configs/app.config'
import { TOKEN_TYPE, REQUEST_HEADER_AUTH_KEY } from '@/constants/api.constant'
import { PERSIST_STORE_NAME } from '@/constants/app.constant'
import deepParseJson from '@/utils/deepParseJson'
import store, { signOutSuccess } from '../store'
import { toast } from '@/components/ui'
import Notification from '@/components/ui/Notification'

const unauthorizedCode = [401]

const sleep = () => new Promise((resolve) => setTimeout(resolve, 500))

const BaseService = axios.create({
    timeout: 60000,
    baseURL: appConfig.apiPrefix,
})

BaseService.interceptors.request.use(
    (config) => {
        const rawPersistData = localStorage.getItem(PERSIST_STORE_NAME)
        const persistData = deepParseJson(rawPersistData)

        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        let accessToken = (persistData as any).auth.session.token

        if (!accessToken) {
            const { auth } = store.getState()
            accessToken = auth.session.token
        }

        if (accessToken) {
            config.headers[
                REQUEST_HEADER_AUTH_KEY
            ] = `${TOKEN_TYPE}${accessToken}`
        }

        return config
    },
    (error) => {
        return Promise.reject(error)
    }
)

BaseService.interceptors.response.use(
    async (response) => {
        if (process.env.NODE_ENV === 'development') await sleep()
        return response
    },
    (error) => {
        const { response } = error

        if (
            response &&
            unauthorizedCode.includes(response.status) &&
            // If the session expires there is no base error response object (only the code is set to 401 by identity server)
            !response?.data
        ) {
            store.dispatch(signOutSuccess())
            toast.push(
                <Notification
                    title="Сесијата истече"
                    type="info"
                    duration={3500}
                >
                    {/* cSpell: disable */}
                    Вашата сесија истече. Најавете се повторно за да продолжите
                    {/* cSpell: enable */}
                </Notification>,
                {
                    placement: 'top-center',
                }
            )
        }

        if (response && response.status == 400) {
            toast.push(
                <Notification
                    title="Настана грешка при валидација"
                    type="danger"
                    duration={3500}
                >
                    {/* cSpell: disable */}
                    Проверете ја вашата форма и обидете се повторно
                    <br />
                    {/* cSpell: enable */}
                </Notification>,
                {
                    placement: 'top-center',
                }
            )
        }

        if (response && response.status == 500) {
            toast.push(
                <Notification
                    title="Настана грешка при комуникацијата со ReservationTravel API"
                    type="danger"
                    duration={3500}
                >
                    {/* cSpell: disable */}
                    ReservationTravel API не враќа одговор на пуштеното барање.
                    <br />
                    Обидете се подоцна.
                    {/* cSpell: enable */}
                </Notification>,
                {
                    placement: 'top-center',
                }
            )
        }

        if ((response && response.status == 422) || response.status == 503) {
            toast.push(
                <Notification
                    title="Проблем во комуникацијата со екстерното API"
                    type="danger"
                    duration={3500}
                >
                    {/* cSpell: disable */}
                    Екстерното API не враќа одговор на пуштеното барање.
                    <br />
                    Обидете се подоцна.
                    {/* cSpell: enable */}
                </Notification>,
                {
                    placement: 'top-center',
                }
            )
        }

        return Promise.reject(error)
    }
)

export default BaseService
