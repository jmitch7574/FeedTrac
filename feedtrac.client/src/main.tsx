import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter, Route, Routes } from "react-router";
import "./index.css";
import SsignUp from "./pages/auth/students/SignUp.tsx";
import SsignIn from "./pages/auth/students/SignIn.tsx";

import TsignUp from "./pages/auth/teachers/SignUp.tsx";
import TsignIn from "./pages/auth/teachers/SignIn.tsx";
import Dashboard from "./components/dashboard/dashboard";
import RequireAuth from "@/components/auth/requireAuth.tsx";
import InsufficientPermission from "@/pages/InsufficientPermission.tsx";
import Options from "@/pages/Options.tsx";
import { Toaster } from "./components/ui/sonner.tsx";
import ForgotPasswordPage from "@/pages/ForgotPasswordPage.tsx";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <Toaster />
    <BrowserRouter>
      <Routes>
        <Route path='/student'>
          // -- route for students
          <Route path='/student/signup' element={<SsignUp />} />
          <Route path='/student/signin' element={<SsignIn />} />
        </Route>
        
        <Route path='/insufficientPermissions' element={<InsufficientPermission />}/>
        <Route path='/reset' element={<ForgotPasswordPage />} />
        
        // -- route for teachers
        <Route path='/teacher/signin' element={<TsignIn />} />

        // -- Require auth
        <Route element={<RequireAuth level={0}/>}>
          <Route path='/' element={<Dashboard />} />
          <Route path='*' element={<h1>404</h1>} />
          <Route path='/options' element={<Options />} />
        </Route>
        
      </Routes>
    </BrowserRouter>
  </StrictMode>
);
