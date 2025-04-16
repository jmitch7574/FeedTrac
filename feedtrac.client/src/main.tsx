import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter, Route, Routes } from "react-router";


import App from "./App.tsx";
import SignUp from "./pages/auth/SignUp.tsx";
import SignIn from "./pages/auth/SignIn.tsx";

import './index.css'
import Home from "./pages/StaffHomePage"
import CreatingModules from './pages/CreatingModules.tsx';
import CurrentModules from './pages/CurrentModules.jsx';
import Tickets from './pages/Tickets.jsx';

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <BrowserRouter>
      <Routes>
        <Route path='/' element={<Home />} />
        <Route path='*' element={<h1>404</h1>} />

        <Route path='/signup' element={<SignUp />} />
        <Route path='/signin' element={<SignIn />} />


        <Route path="/Creating_Modules" element={<CreatingModules />} />
        <Route path="/Current_Modules" element={<CurrentModules />} />
        <Route path="/Home" element={<Home />} />
        <Route path="/Tickets" element={<Tickets />} />

      </Routes>
    </BrowserRouter>
  </StrictMode>
);
