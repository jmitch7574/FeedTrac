import { useNavigate } from "react-router";
import { Button } from "../ui/button";

const OptionsButton = () => {
  const navigate = useNavigate();

  const handleOptions = async () => {
    navigate("/options");
  };

  return <Button onClick={handleOptions}>Options</Button>;
};

export default OptionsButton;
