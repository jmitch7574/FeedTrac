import { useState } from "react";
import { cn } from "@/lib/utils";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Link } from "react-router";
import type { teacherRegister } from "@/types/Index";
import { registerTeacher } from "@/hooks/useAuth";

export function CSignUp({ className, ...props }: React.ComponentProps<"div">) {
  const [FirstName, setFirstName] = useState("test");
  const [LastName, setLastName] = useState("test");
  const [Email, setEmail] = useState("teacher@lincoln.ac.uk");

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    const payload: teacherRegister = {
      FirstName,
      LastName,
      Email,
    };

    try {
      const res = await registerTeacher(payload);
      console.log("Success:", res.token); // store token or redirect
    } catch (err) {
      console.error("Registration failed:", err);
    }
  };

  return (
    <div className={cn("flex flex-col gap-6", className)} {...props}>
      <Card>
        <CardHeader>
          <CardTitle className='text-2xl'>Create a teacher's account</CardTitle>
          <CardDescription>Enter your details below to sign up for an account</CardDescription>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit}>
            <div className='flex flex-col gap-6'>
              <div className='flex flex-row gap-2'>
                <div className='grid gap-3'>
                  <Label htmlFor='firstName'>First Name</Label>
                  <Input id='firstName' type='text' placeholder='John' required value={FirstName} onChange={(e) => setFirstName(e.target.value)} />
                </div>
                <div className='grid gap-3'>
                  <Label htmlFor='lastName'>Last Name</Label>
                  <Input id='lastName' type='text' placeholder='Doe' required value={LastName} onChange={(e) => setLastName(e.target.value)} />
                </div>
              </div>
              <div className='grid gap-3'>
                <Label htmlFor='email'>Email</Label>
                <Input id='email' type='email' placeholder='johndoe@example.com' required value={Email} onChange={(e) => setEmail(e.target.value)} />
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
