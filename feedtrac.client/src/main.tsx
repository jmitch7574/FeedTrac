import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter, Route, Routes } from "react-router";
import "./index.css";
import App from "./App.tsx";
import SsignUp from "./pages/auth/students/SignUp.tsx";
import SsignIn from "./pages/auth/students/SignIn.tsx";

import TsignUp from "./pages/auth/teachers/SignUp.tsx";
import TsignIn from "./pages/auth/teachers/SignIn.tsx";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <BrowserRouter>
      <Routes>
        <Route path='/' element={<App />} />
        <Route path='*' element={<h1>404</h1>} />
        // -- route for students
        <Route path='/student'>
          <Route path='/student/signup' element={<SsignUp />} />
          <Route path='/student/signin' element={<SsignIn />} />
        </Route>
        // -- route for teachers
        <Route path='/teacher'>
          <Route path='/teacher/signup' element={<TsignUp />} />
          <Route path='/teacher/signin' element={<TsignIn />} />
        </Route>
      </Routes>
    </BrowserRouter>
  </StrictMode>
);
