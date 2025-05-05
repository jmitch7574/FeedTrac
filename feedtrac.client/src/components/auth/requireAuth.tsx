import { useEffect, useState } from "react";
import { useNavigate, Outlet } from "react-router-dom";
import { apiClient } from "@/lib/apiClient";

const RequireAuth = () => {
    const navigate = useNavigate();
    const [checking, setChecking] = useState(true);

    useEffect(() => {
        const checkAuth = async () => {
            try {
                const res = await apiClient.get("/identity");
                if (res.data.status === "NotAuthenticated") {
                    navigate("/student/signin", { replace: true });
                }
            } catch (err) {
                console.error("Error checking auth:", err);
                navigate("/student/signin", { replace: true });
            } finally {
                setChecking(false);
            }
        };
        checkAuth();
    }, [navigate]);

    if (checking) return <div>Checking authentication...</div>;

    return <Outlet />;
};

export default RequireAuth;
