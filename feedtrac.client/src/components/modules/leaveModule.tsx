import { studentLeaveModule } from "@/hooks/useModules";
import { useState } from "react";

const LeaveModule = ({ moduleId }: { moduleId: number }) => {
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
      // Optionally: trigger refresh or notify parent component
    } catch (err) {
      console.error("Failed to delete module:", err);
      setError("Failed to delete module");
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
