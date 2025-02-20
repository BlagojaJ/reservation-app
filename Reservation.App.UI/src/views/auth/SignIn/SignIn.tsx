import SignInForm from './SignInForm'

const SignIn = () => {
    return (
        <>
            <div className="mb-8">
                <h3 className="mb-1">Добредојде!</h3>
                <p>Внесете име и лозинка за да се најавите!</p>
            </div>
            <SignInForm disableSubmit={false} />
        </>
    )
}

export default SignIn
