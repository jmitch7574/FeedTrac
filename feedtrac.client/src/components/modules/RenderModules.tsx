import { getAllModulesForUser } from "@/hooks/useModules";
import { Module, ModuleResponse } from "@/types/Index";
import { useEffect, useState } from "react";
import ModuleCard from "./ModuleCard";
import CreateModule from "./CreateModule";
import useRole from "@/hooks/useRole";
import JoinModule from "./joinModule";
import RenderOnlyAdmin from "./renderOnlyAdmin";

const RenderModules = () => {
  const [modules, setModules] = useState<Module[]>([]);
  const [error, setError] = useState<string | null>(null);
  const role = useRole(); // Get the user's role using the custom hook
  const [refresh, setRefresh] = useState<boolean>(false);

  useEffect(() => {
    const fetchModules = async () => {
      try {
        const res: ModuleResponse = await getAllModulesForUser();
        setModules(res.modules);
        // console.log("Modules:", res);
      } catch (err) {
        console.error("Failed to fetch modules:", err);
        setError("Failed to load modules");
      }
    };

    fetchModules();
  }, [refresh]);

  // Callback to update modules when a new one is created
  const handleModuleCreation = async () => {
    setRefresh(refresh => !refresh);
  };

  // Callback to handle joining a module
  const handleModuleJoined = async () => {
    setRefresh(refresh => !refresh);
  };

  const handleModuleDestroy = () => {
    setRefresh(refresh => !refresh);
  }
  
  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className='flex flex-col flex-wrap'>
      <p className='text-2xl capitalize text-wrap'>Your Modules:</p>
      <ul className='flex flex-row gap-4 flex-wrap'>
        <JoinModule onModuleJoined={handleModuleJoined} />

        {modules.map((mod) => (
          <ModuleCard key={mod.id} id={mod.id} moduleName={String(mod.name)} moduleCode={String(mod.joinCode)} onModuleDestroy={handleModuleDestroy} />
        ))}

        {/* Show the CreateModule button for admins and teachers */}
        {(role === "admin" || role === "teacher") && <CreateModule onModuleCreated={handleModuleCreation} />}
      </ul>

      {role === "admin" && <RenderOnlyAdmin onModuleDestroy={handleModuleDestroy} refresh={refresh} />}
    </div>
  );
};

export default RenderModules;
