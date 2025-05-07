import { cn } from "@/lib/utils";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { useState } from "react";
import {ResetPasswordRequest} from "@/types/Index";
import {resetPasswordRequest} from "@/hooks/useAuth";
import ErrorBox from "@/components/ui/ErrorBox.tsx";
import {Button} from "@/components/ui/button.tsx";

export function ResetPasswordForm({ className, ...props }: React.ComponentProps<"div">) {
    const [OldPassword, setOldPassword] = useState("");
    const [NewPassword, setNewPassword] = useState("");
    let [error, setError] = useState("");

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        console.log("Reset password form submitted successfully");

        const payload: ResetPasswordRequest = {
            currentPassword: OldPassword,
            newPassword: NewPassword,
        };

        try {
            await resetPasswordRequest(payload);
            console.log("Success");
            setOldPassword("");
            setNewPassword("");
        } catch (err : any) {
            console.error("Sign in failed:", err);
            setError(err.response.data.error);
        }
    };

    return (
        <div className={cn("flex flex-col gap-6", className)} {...props}>
            <Card>
                <CardHeader>
                    <CardTitle className='text-2xl'>Change Password</CardTitle>
                    <CardDescription>Update the password you use to sign in</CardDescription>
                </CardHeader>
                <CardContent>
                    <form onSubmit={handleSubmit}>
                        <div className='flex flex-col gap-6'>
                            <div className='grid gap-3'>
                                <Label htmlFor='oldPassword'>Current Password</Label>
                                <Input id='password' type='password' placeholder='Enter your current password' required value={OldPassword} onChange={(e) => setOldPassword(e.target.value)} />
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
                    </form>
                </CardContent>
            </Card>
            <ErrorBox errors={error.length > 0 ? [error] : []}></ErrorBox>
        </div>
    );
}

export default ResetPasswordForm;