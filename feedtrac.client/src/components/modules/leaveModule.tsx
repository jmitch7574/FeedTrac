import { studentLeaveModule } from "@/hooks/useModules";
import { useState } from "react";
import { toast } from "sonner";

const LeaveModule = ({ moduleId, onModuleDestroy }: { moduleId: number, onModuleDestroy: () => void }) => {
  const [error, setError] = useState<string | null>(null);

  const handleDelete = async (e: React.MouseEvent) => {
    e.stopPropagation();
    e.preventDefault();

    if (!moduleId) {
      setError("Module ID is required");
      return;
    }

    try {
      await studentLeaveModule(moduleId);
      toast.success("Module left successfully!");
      onModuleDestroy()
    } catch {
      toast.error("Failed to leave module");
    }
  };

  return (
    <>
      <span className='absolute top-1 right-1 w-5 h-5 rounded bg-black cursor-pointer cl' onClick={handleDelete} title='Leave Module' />
      {error && <p className='text-red-400 text-sm'>{error}</p>}
    </>
  );
};

export default LeaveModule;
