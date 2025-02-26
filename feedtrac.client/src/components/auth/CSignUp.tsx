import { cn } from "@/lib/utils";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Link } from "react-router";

export function CSignUp({ className, ...props }: React.ComponentProps<"div">) {
  return (
    <div className={cn("flex flex-col gap-6", className)} {...props}>
      <Card>
        <CardHeader>
          <CardTitle className='text-2xl'>Create an account</CardTitle>
          <CardDescription>Enter your details below to signup for an account</CardDescription>
        </CardHeader>
        <CardContent>
          <form>
            <div className='flex flex-col gap-6'>
              <div className='flex flex-row gap-2'>
                <div className='grid gap-3'>
                  <Label htmlFor='firstName'>FirstName</Label>
                  <Input id='firstName' type='text' placeholder='John' required />
                </div>
                <div className='grid gap-3'>
                  <Label htmlFor='lastName'>LastName</Label>
                  <Input id='lastName' type='text' placeholder='Doe' required />
                </div>
              </div>
              <div className='grid gap-3'>
                <Label htmlFor='email'>Email</Label>
                <Input id='email' type='email' placeholder='JohnDoe@example.com' required />
              </div>
              <div className='grid gap-3'>
                <div className='flex items-center'>
                  <Label htmlFor='password'>Password</Label>
                  <a href='#' className='ml-auto inline-block text-sm underline-offset-4 hover:underline'>
                    Forgot your password?
                  </a>
                </div>
                <Input id='password' type='password' placeholder='Enter your password' required />
              </div>
              <div className='flex flex-col gap-3'>
                <Button type='submit' className='w-full'>
                  Create account
                </Button>
              </div>
            </div>
            <div className='mt-4 text-center text-sm'>
              Have an account?{" "}
              <Link to='/signin' className='underline underline-offset-4'>
                Sign in
              </Link>
            </div>
          </form>
        </CardContent>
      </Card>
    </div>
  );
}
