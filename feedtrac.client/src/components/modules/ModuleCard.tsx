import { Link } from "react-router";
import { Button } from "../ui/button";
import DeleteModule from "../auth/admin/DeleteModule";
import useRole from "@/hooks/useRole";
import LeaveModule from "./leaveModule";

interface ModuleCardProps {
  moduleName: string;
  moduleCode?: string;
  id: number;
}

const ModuleCard: React.FC<ModuleCardProps> = ({ moduleName, moduleCode, id }) => {
  const role = useRole(); // Get the user's role using the custom hook

  return (
    <Link to={`/module/${id}`}>
      <Button className='relative h-[192px] w-[328px] text-black bg-gray-300 rounded flex flex-col  justify-center items-center hover:bg-gray-400 transition duration-200 ease-in-out'>
        {role === "admin" && <DeleteModule moduleId={id} />}
        {role === "student" && <LeaveModule moduleId={id} />}
        <p className='text-2xl capitalize text-wrap'>{moduleName}</p>
        <p className='font-normal'>Join Code: {moduleCode}</p>
      </Button>
    </Link>
  );
};

export default ModuleCard;
