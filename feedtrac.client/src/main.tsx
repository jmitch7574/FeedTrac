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

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <BrowserRouter>
      <Routes>
        <Route path='/student'>
          <Route path='/student/signup' element={<SsignUp />} />
          <Route path='/student/signin' element={<SsignIn />} />
        </Route>
        // -- route for teachers
        <Route path='/teacher'>
          <Route path='/teacher/signup' element={<TsignUp />} />
          <Route path='/teacher/signin' element={<TsignIn />} />
        </Route>

        <Route element={<RequireAuth />}>
          <Route path='/' element={<Dashboard />} />
          <Route path='*' element={<h1>404</h1>} />
        </Route>
        // -- route for students
      </Routes>
    </BrowserRouter>
  </StrictMode>
);
