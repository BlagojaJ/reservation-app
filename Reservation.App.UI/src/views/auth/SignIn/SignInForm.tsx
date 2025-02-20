import Input from '@/components/ui/Input'
import Button from '@/components/ui/Button'
import Checkbox from '@/components/ui/Checkbox'
import { FormItem, FormContainer } from '@/components/ui/Form'
import Alert from '@/components/ui/Alert'
import PasswordInput from '@/components/shared/PasswordInput'
import useTimeOutMessage from '@/utils/hooks/useTimeOutMessage'
import useAuth from '@/utils/hooks/useAuth'
import { Field, Form, Formik } from 'formik'
import * as Yup from 'yup'
import type { CommonProps } from '@/@types/common'

interface SignInFormProps extends CommonProps {
    disableSubmit?: boolean
}

type SignInFormSchema = {
    username: string
    password: string
    rememberMe: boolean
}

const validationSchema = Yup.object().shape({
    username: Yup.string().required('Внесете го вашето корисничко име'),
    password: Yup.string().required('Внесете ја вашата лозинка'),
    rememberMe: Yup.bool(),
})

const SignInForm = (props: SignInFormProps) => {
    const { disableSubmit = false, className } = props

    const [message, setMessage] = useTimeOutMessage()

    const { signIn } = useAuth()

    const username = localStorage.getItem('username') ?? ''

    const deleteLocalStorageOnPageClose = () => {
        window.addEventListener('beforeunload', () => {
            localStorage.removeItem('username')
            localStorage.removeItem('admin')
        })
    }

    const onSignIn = async (
        values: SignInFormSchema,
        setSubmitting: (isSubmitting: boolean) => void
    ) => {
        const { username, password, rememberMe } = values
        setSubmitting(true)

        if (rememberMe) {
            localStorage.setItem('username', username)
        }

        if (!rememberMe) {
            deleteLocalStorageOnPageClose()
        }

        const result = await signIn({ username, password })

        if (result?.status === 'failed') {
            setMessage(result.message)
        }
        setSubmitting(false)
    }

    return (
        <div className={className}>
            {message && (
                <Alert showIcon className="mb-4" type="danger">
                    <>{message}</>
                </Alert>
            )}
            <Formik
                initialValues={{
                    username,
                    password: '',
                    rememberMe: true,
                }}
                validationSchema={validationSchema}
                onSubmit={(values, { setSubmitting }) => {
                    if (!disableSubmit) {
                        onSignIn(values, setSubmitting)
                    } else {
                        setSubmitting(false)
                    }
                }}
            >
                {({ touched, errors, isSubmitting }) => (
                    <Form>
                        <FormContainer>
                            <FormItem
                                label="Име"
                                invalid={
                                    (errors.username &&
                                        touched.username) as boolean
                                }
                                errorMessage={errors.username}
                            >
                                <Field
                                    type="text"
                                    autoComplete="off"
                                    name="username"
                                    placeholder="Корисничко име"
                                    component={Input}
                                />
                            </FormItem>
                            <FormItem
                                label="Лозинка"
                                invalid={
                                    (errors.password &&
                                        touched.password) as boolean
                                }
                                errorMessage={errors.password}
                            >
                                <Field
                                    autoComplete="off"
                                    name="password"
                                    placeholder="Лозинка"
                                    component={PasswordInput}
                                />
                            </FormItem>
                            <div className="flex justify-between mb-9">
                                <Field
                                    className="mb-0"
                                    name="rememberMe"
                                    component={Checkbox}
                                >
                                    Запомни ме
                                </Field>
                            </div>
                            <Button
                                block
                                loading={isSubmitting}
                                variant="solid"
                                type="submit"
                            >
                                {isSubmitting ? 'Најавување...' : 'Најави се'}
                            </Button>
                        </FormContainer>
                    </Form>
                )}
            </Formik>
        </div>
    )
}

export default SignInForm
