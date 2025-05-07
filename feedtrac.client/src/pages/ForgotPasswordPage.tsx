import ForgotPasswordForm from "@/components/auth/ForgotPasswordForm.tsx";


const ForgotPasswordPage = () => {
    const logo = "/University_of_Lincoln_logo_landscape.png";

    return (
        <div className='flex min-h-svh min-w-screen items-center justify-center p-6 md:p-10'>
            <div className='w-full max-w-sm'>
                <div className='flex items-center justify-center rounded-md text-primary-foreground mb-4 px-4 '>
                    <img src={logo} className='w-full h-full object-cover' />
                </div>
                
                <p className='text-2xl capitalize text-wrap p-5'>Forgot Password</p>
                
                <ForgotPasswordForm />
            </div>
        </div>
    );
};

export default ForgotPasswordPage;
