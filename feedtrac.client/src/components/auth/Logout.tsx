import { userLogout } from "@/hooks/useAuth";
import { useNavigate } from "react-router";
import { Button } from "../ui/button";

const Logout = () => {
  const navigate = useNavigate();

  const handleLogout = async () => {
    try {
      await userLogout();
      navigate("/logouted");
    } catch (error) {
      console.error("Logout failed:", error);
    }
  };

  return <Button onClick={handleLogout}>Logout</Button>;
};

export default Logout;
