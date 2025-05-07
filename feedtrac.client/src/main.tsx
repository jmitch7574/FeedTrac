import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter, Route, Routes } from "react-router";
import "./index.css";
import SsignUp from "./pages/auth/students/SignUp.tsx";
import SsignIn from "./pages/auth/students/SignIn.tsx";

import TsignIn from "./pages/auth/teachers/SignIn.tsx";
import Dashboard from "./components/dashboard/dashboard";
import RequireAuth from "@/components/auth/requireAuth.tsx";
import InsufficientPermission from "@/pages/InsufficientPermission.tsx";
import Options from "@/pages/Options.tsx";
import { Toaster } from "./components/ui/sonner.tsx";
import ModuleDetail from "@/components/modules/ModuleDetails.tsx";
import TicketDetail from "@/components/tickets/TicketDetails.tsx";
import Navigation from "@/components/ui/Navigation.tsx";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <Toaster />
    <BrowserRouter>
      <Navigation />
      <Routes>
        <Route path='/student'>
          // -- route for students
          <Route path='/student/signup' element={<SsignUp />} />
          <Route path='/student/signin' element={<SsignIn />} />
        </Route>
        <Route path='/insufficientPermissions' element={<InsufficientPermission />} />
        // -- route for teachers
        <Route path='/teacher/signin' element={<TsignIn />} />
        // -- Require auth
        <Route element={<RequireAuth level={0} />}>
          <Route path='/' element={<Dashboard />} />
          <Route path='/module/:moduleId' element={<ModuleDetail />} />
          <Route path='/tickets/:ticketId' element={<TicketDetail />} />
          <Route path='*' element={<h1>404</h1>} />
          <Route path='/options' element={<Options />} />
        </Route>
      </Routes>
    </BrowserRouter>
  </StrictMode>
);
