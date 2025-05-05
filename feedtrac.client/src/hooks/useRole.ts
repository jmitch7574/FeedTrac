import { useState, useEffect } from "react";
import { getUserInfo } from "@/hooks/useAuth"; // Import the hook that fetches user info
import { IdentityResponse } from "@/types/Index"; // Import the IdentityResponse type

const useRole = () => {
  const [role, setRole] = useState<string | null>(null);

  useEffect(() => {
    const fetchUserInfo = async () => {
      const userInfo: IdentityResponse = await getUserInfo();

      // Check for the specific roles in the 'status'
      if (userInfo?.status === "AuthenticatedTeacher") {
        setRole("teacher");
      } else if (userInfo?.status === "AuthenticatedAdmin") {
        setRole("admin");
      } else if (userInfo?.status === "AuthenticatedStudent") {
        setRole("student");
      } else {
        setRole(null); // No valid role or user not authenticated
      }
    };

    fetchUserInfo();
  }, []);

  return role;
};

export default useRole;
