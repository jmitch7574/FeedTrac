import { cn } from "@/lib/utils";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { useState } from "react";;
import { Link } from "react-router";
import ErrorBox from "@/components/ui/ErrorBox.tsx";
import {Button} from "@/components/ui/button.tsx";
import {forgotPasswordRequest, forgotPasswordRequestFollowUp} from "@/hooks/useAuth.ts";
import {ForgotPasswordRequest, ForgotPasswordFollowupRequest} from "@/types/Index";
import { toast } from "sonner";

export function ForgotPasswordForm({ className, ...props }: React.ComponentProps<"div">) {
    const [email, setEmail] = useState<string>("");
    const [resetCode, setResetCode] = useState<string>("");
    const [NewPassword, setNewPassword] = useState("");
    let [error, setError] = useState("");

    const sendEmail = async(e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        
        const payload: ForgotPasswordRequest = {
            email: email,
        }
        
        try{
            forgotPasswordRequest(payload);
            console.log("Success");
            toast.success("Your code is on its way, please check your emails");
        } catch (err : any) {
            console.error("reset failed:", err);
            setError(err.response.data.error);
            toast.error("Code failed to send:  " + err.response.data.error);
        }
    }
    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        console.log("Reset password form submitted successfully");

        const payload: ForgotPasswordFollowupRequest = {
            email: email,
            resetCode: resetCode,
            newPassword: NewPassword,
        };

        try {
            await forgotPasswordRequestFollowUp(payload);
            console.log("Success");
            toast.success("Your password has been reset successfully");
        } catch (err : any) {
            console.error("reset failed:", err);
            setError(err.response.data.error);
            toast.error("Failed to reset password:  " + err.response.data.error);
        }
    };

    return (
        <div className={cn("flex flex-col gap-6", className)} {...props}>
            <Card>
                <CardHeader>
                    <CardTitle className='text-2xl'>Get Reset Code</CardTitle>
                    <CardDescription>Enter your email to get your emergency reset code</CardDescription>
                </CardHeader>
                <CardContent>
                    <form onSubmit={sendEmail}>
                        <div className='flex flex-col gap-6'>
                            <div className='grid gap-3'>
                                <Label htmlFor='email'>Email</Label>
                                <Input id='email' type='email' required value={email} onChange={(e) => setEmail(e.target.value)} />
                            </div>
                            <div className='flex flex-col gap-3'>
                                <Button type='submit' className='w-full'>
                                    Get Code
                                </Button>
                            </div>
                        </div>
                    </form>
                </CardContent>
            </Card>
            <Card>
                <CardHeader>
                    <CardTitle className='text-2xl'>Reset Password</CardTitle>
                    <CardDescription>Use your emergency code to reset your password</CardDescription>
                </CardHeader>
                <CardContent>
                    <form onSubmit={handleSubmit}>
                        <div className='flex flex-col gap-6'>
                            <div className='grid gap-3'>
                                <Label htmlFor='email'>Email</Label>
                                <Input id='email' type='email' required value={email} onChange={(e) => setEmail(e.target.value)} />
                            </div>
                            <div className='grid gap-3'>
                                <Label htmlFor='resetCode'>Reset Code</Label>
                                <Input id='code' type='code' required value={resetCode} onChange={(e) => setResetCode(e.target.value)} />
                            </div>
                            <div className='grid gap-3'>
                                <Label htmlFor='newPassword'>New Password</Label>
                                <Input id='password' type='password' placeholder='Enter your new password' required value={NewPassword} onChange={(e) => setNewPassword(e.target.value)} />
                            </div>
                            <div className='flex flex-col gap-3'>
                                <Button type='submit' className='w-full'>
                                    Update Password
                                </Button>
                            </div>
                        </div>
                        <div className='mt-4 text-center text-sm'>
                            <Link to='/student/signin' className='underline underline-offset-4'>
                                Back to Sign in
                            </Link>
                        </div>
                    </form>
                </CardContent>
            </Card>
            <ErrorBox errors={error.length > 0 ? [error] : []}></ErrorBox>
        </div>
    );
}

export default ForgotPasswordForm;