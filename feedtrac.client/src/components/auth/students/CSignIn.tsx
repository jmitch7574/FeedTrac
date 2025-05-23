import { cn } from "@/lib/utils";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Link } from "react-router";
import { useState } from "react";
import { studentLogin } from "@/types/Index";
import { loginStudent } from "@/hooks/useAuth";
import { useNavigate } from "react-router";
import { toast } from "sonner";

export function CSignIn({ className, ...props }: React.ComponentProps<"div">) {
  const [Email, setEmail] = useState("");
  const [Password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    console.log("Sign in form submitted successfully");

    const payload: studentLogin = {
      Email,
      Password,
      rememberMe: true,
    };

    try {
      await loginStudent(payload);
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
          <CardTitle className='text-2xl'>Sign into your student account</CardTitle>
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
                  <Link to='/reset' className='ml-auto inline-block text-sm underline-offset-4 hover:underline'>
                    Forgot Password?
                  </Link>
                </div>
                <Input id='password' type='password' placeholder='Enter your password' required value={Password} onChange={(e) => setPassword(e.target.value)} />
              </div>
              <div className='flex flex-col gap-3'>
                <Button type='submit' className='w-full'>
                  Sign into your account
                </Button>
              </div>
            </div>
            <div className='mt-4 text-center text-sm'>
              Don&apos;t Have an account?{" "}
              <Link to='/student/signup' className='underline underline-offset-4'>
                Sign up
              </Link>
            </div>
            <div className='mt-4 text-center text-sm'>
              Are you a teacher?{" "}
              <Link to='/teacher/signin' className='underline underline-offset-4'>
                Sign in
              </Link>
            </div>
          </form>
        </CardContent>
      </Card>
    </div>
  );
}
