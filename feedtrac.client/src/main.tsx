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

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <BrowserRouter>
      <Routes>
        <Route path='/student'>
          // -- route for students
          <Route path='/student/signup' element={<SsignUp />} />
          <Route path='/student/signin' element={<SsignIn />} />
        </Route>
        
        <Route path='/insufficientPermissions' element={<InsufficientPermission />}/>
        
        // -- route for teachers
        <Route path='/teacher/signin' element={<TsignIn />} />

        // -- Require auth
        <Route element={<RequireAuth level={0}/>}>
          <Route path='/' element={<Dashboard />} />
          <Route path='*' element={<h1>404</h1>} />
        </Route>
        
        // -- routes for Admins
        <Route element={<RequireAuth level={2}/>}>
          <Route path='/teacher/signup' element={<TsignUp />} />
        </Route>
        
      </Routes>
    </BrowserRouter>
  </StrictMode>
);
