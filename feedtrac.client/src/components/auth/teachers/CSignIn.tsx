import { cn } from "@/lib/utils";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { useState } from "react";
import { Link } from "react-router";
import { teacherLogin as teacherLoginType } from "@/types/Index";
import { loginTeacher } from "@/hooks/useAuth";
import { useNavigate } from "react-router";
import { toast } from "sonner";

export function CSignIn({ className, ...props }: React.ComponentProps<"div">) {
  const [Email, setEmail] = useState("feedtrac-admin@lincoln.ac.uk");
  const [Password, setPassword] = useState("Password123!");
  const [twoFactorCode, setTwoFactorCode] = useState("");
  // const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    // console.log("Sign in form submitted successfully");

    const payload: teacherLoginType = {
      Email,
      Password,
      rememberMe: true,
      twoFactorCode,
    };

    try {
      await loginTeacher(payload);
      toast.success("Sign in successful!");
      navigate("/"); // redirect to home page
    } catch (err: any) {
      toast.error("Sign in failed: " + (err.response?.data?.error || err.message));
    }
  };

  return (
    <div className={cn("flex flex-col gap-6", className)} {...props}>
      <Card>
        <CardHeader>
          <CardTitle className='text-2xl'>Sign into your teacher's account</CardTitle>
          <CardDescription>Enter your details below to signin to your account</CardDescription>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit}>
            <div className='flex flex-col gap-6'>
              <div className='grid gap-3'>
                <Label htmlFor='email'>Email</Label>
                <Input id='email' type='email' placeholder='JohnDoe@example.com' required value={Email} onChange={(e) => setEmail(e.target.value)} />
              </div>
              <div className='grid gap-3'>
                <div className='flex items-center'>
                  <Label htmlFor='password'>Password</Label>
                  <a href='#' className='ml-auto inline-block text-sm underline-offset-4 hover:underline'>
                    Forgot your password?
                  </a>
                </div>
                <Input id='password' type='password' placeholder='Enter your password' required value={Password} onChange={(e) => setPassword(e.target.value)} />
              </div>
              <div className='grid gap-3'>
                <Label htmlFor='twoFactorCode'>Code</Label>
                <Input id='twoFactorCode' type='number' placeholder='000000' required value={twoFactorCode} onChange={(e) => setTwoFactorCode(e.target.value)} />
              </div>
              <div className='flex flex-col gap-3'>
                <Button type='submit' className='w-full'>
                  Sign into your account
                </Button>
              </div>
            </div>
            <div className='mt-4 text-center text-sm'>
              Are you a student?{" "}
              <Link to='/student/signin' className='underline underline-offset-4'>
                Sign in here
              </Link>
            </div>
          </form>
        </CardContent>
      </Card>
      {/* <ErrorBox errors={error.length > 0 ? [error] : []}></ErrorBox> */}
    </div>
  );
}
