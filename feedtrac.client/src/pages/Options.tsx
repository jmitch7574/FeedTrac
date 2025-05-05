import ResetPasswordForm from "@/components/auth/ResetPasswordForm.tsx";
import useRole from "@/hooks/useRole.ts";
import {CSignUp} from "@/components/auth/teachers/CSignUp.tsx";

const Options = () => {
    const logo = "/University_of_Lincoln_logo_landscape.png";
    const role = useRole();

    return (
        <div className='flex min-h-svh min-w-screen items-center justify-center p-6 md:p-10'>
            <div className='w-full max-w-sm'>
                <div className='flex items-center justify-center rounded-md text-primary-foreground mb-4 px-4 '>
                    <img src={logo} className='w-full h-full object-cover' />
                </div>
                <ResetPasswordForm/>

                {role === "admin"? <CSignUp></CSignUp> : null}
            </div>
        </div>
    );
};

export default Options;
