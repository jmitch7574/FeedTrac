import { Link } from "react-router";
import DeleteModule from "../auth/admin/DeleteModule";
import useRole from "@/hooks/useRole";
import LeaveModule from "./leaveModule";
import CopyToClipboard from "./CopyToClipboard";

interface ModuleCardProps {
  moduleName: string;
  moduleCode?: string;
  id: number;
  onModuleDestroy: () => void;
}

const ModuleCard: React.FC<ModuleCardProps> = ({ moduleName, moduleCode, id, onModuleDestroy }) => {
  const role = useRole(); // Get the user's role using the custom hook

  return (
    <Link to={`/module/${id}`}>
      <div className='relative h-[192px] w-[328px] text-black bg-gray-300 rounded flex flex-col  justify-center items-center hover:bg-gray-400 transition duration-200 ease-in-out'>
        {role === "admin" && <DeleteModule moduleId={id} onModuleDestroy={onModuleDestroy} />}
        {role === "student" && <LeaveModule moduleId={id} onModuleDestroy={onModuleDestroy} />}
        <p className='text-2xl capitalize text-wrap align-middle text-center p-3'>{moduleName}</p>
        <div className='flex flex-row gap-2 items-center mt-2'>
          <CopyToClipboard text={String(moduleCode)} />
          <p className='font-normal text-wrap'>{moduleCode}</p>
        </div>
      </div>
    </Link>
  );
};

export default ModuleCard;
