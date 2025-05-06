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
  }, []);

  // Callback to update modules when a new one is created
  const handleModuleCreation = async (newModule: Module) => {
    setModules((prevModules) => [...prevModules, newModule]);
  };

  // Callback to handle joining a module
  const handleModuleJoined = (joinedModule: Module) => {
    setModules((prevModules) => [...prevModules, joinedModule]);
  };

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className='flex flex-row flex-wrap'>
      <ul className='flex flex-row gap-4 flex-wrap'>
        {modules.map((mod) => (
            <ModuleCard key={mod.id} id={mod.id} moduleName={String(mod.name)} moduleCode={String(mod.joinCode)} />
        ))}

        {/* Show the CreateModule button for admins and teachers */}
        {(role === "admin" || role === "teacher") && modules.length === 0 && <CreateModule onModuleCreated={handleModuleCreation} />}
  
        {role === "admin" && <RenderOnlyAdmin />}
  
        {/* Show the JoinModule button for students even if no modules exist */}
        {(role === "student" || role ==="teacher") && <JoinModule onModuleJoined={handleModuleJoined} />}
      </ul>
    </div>
  );
};

export default RenderModules;
