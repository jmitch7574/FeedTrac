import React, { useState, useEffect } from "react";
import { getAllModules } from "@/hooks/useModules"; // Update path as needed
import { Module, ModuleResponse } from "@/types/Index"; // Update path as needed
import useRole from "@/hooks/useRole"; // Assuming you have a hook to fetch the user's role
import ModuleCard from "./ModuleCard";

const RenderOnlyAdmin = () => {
  const [modules, setModules] = useState<Module[]>([]);
  const [error, setError] = useState<string | null>(null);
  const role = useRole(); // Get the user's role

  useEffect(() => {
    const fetchModules = async () => {
      try {
        const res: ModuleResponse = await getAllModules();
        setModules(res.modules);
      } catch (err) {
        console.error("Failed to fetch modules:", err);
        setError("Failed to load modules");
      }
    };

    if (role === "admin") {
      fetchModules(); // Only fetch modules if the user is an admin
    }
  }, [role]);

  if (role !== "admin") {
    return <p>You do not have permission to view this page.</p>; // Message if user is not admin
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className='flex flex-row flex-wrap'>
      {modules.length === 0 ? (
        <p>No modules available.</p>
      ) : (
        <ul className='flex flex-row gap-4 flex-wrap'>
          {modules.map((mod) => (
            <ModuleCard key={mod.id} id={mod.id} moduleName={String(mod.name)} moduleCode={String(mod.joinCode)} />
          ))}
        </ul>
      )}
    </div>
  );
};

export default RenderOnlyAdmin;
