interface ErrorBoxProps
{
    errors: string[]
}

function ErrorBox({ errors } : ErrorBoxProps) {
    return (
        <div>
            {
                errors.length == 0 ? null:
                <div className='mt-4 text-center text-sm g-red-100 border border-red-500 text-red-700 p-4 rounded'>
                {
                    errors.map((error : any) => (
                        <p>{error}</p>
                    ))
                }
                </div>
            }
        </div>
    )
}

export default ErrorBox