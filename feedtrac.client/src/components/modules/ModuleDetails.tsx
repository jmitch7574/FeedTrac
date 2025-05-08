// src/pages/ModuleDetail.tsx
import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { getModuleInfo } from "@/hooks/useModules";
import useRole from "@/hooks/useRole";
import { Module } from "@/types/Index";
import CreateTicket from "../tickets/CreateTicket";
import ModuleTickets from "../tickets/ModuleTickets";

export default function ModuleDetail() {
  const { moduleId } = useParams<{ moduleId: string }>();
  const [module, setModule] = useState<Module | null>(null);
  const [loading, setLoading] = useState(true);
  const [refresh, setRefresh] = useState(false);
  
  const role = useRole();

  useEffect(() => {
    if (!moduleId) return;
    (async () => {
      try {
        const mod = await getModuleInfo(Number(moduleId));
        setModule(mod);
      } catch (err) {
        console.error("Could not load module info", err);
      } finally {
        setLoading(false);
      }
    })();
  }, [moduleId, refresh]);

  if (loading) return <p>Loading module…</p>;
  if (!module) return <p>Module not found</p>;

  return (
    <section className='h-screen bg-gray-100  flex flex-col'>
      <div className='flex flex-col  h-full  py-4 px-6'>
        <div className='flex flex-row gap-2 items-center justify-between mb-2'>
          <h1 className='text-2xl font-bold capitalize'>{String(module.name)}</h1>
          <p className='text-sm text-gray-600'>Join Code: {module.joinCode}</p>
        </div>
        {role === "student" && <CreateTicket moduleId={module.id} onTicketCreated={() => {setRefresh(refresh => !refresh);}} />}
        <div className='flex flex-col w-full mt-4'>
          <p className='text-2xl mb-2'>Your tickets </p>
          <ModuleTickets moduleId={module.id} refresh={refresh} />
        </div>
      </div>
    </section>
  );
}
