import { deleteModule } from "@/hooks/useModules";
import { useState } from "react";

const DeleteModule = ({ moduleId, onModuleDestroy }: { moduleId: number, onModuleDestroy: () => void }) => {
  const [error, setError] = useState<string | null>(null);

  const handleDelete = async (e: React.MouseEvent) => {
    e.stopPropagation();
    e.preventDefault();

    if (!moduleId) {
      setError("Module ID is required");
      return;
    }

    try {
      await deleteModule(moduleId);
      onModuleDestroy();
      // Optionally: trigger refresh or notify parent component
    } catch (err) {
      console.error("Failed to delete module:", err);
      setError("Failed to delete module");
    }
  };

  return (
    <>
      <span className='absolute top-1 right-1 w-5 h-5 rounded bg-black cursor-pointer cl' onClick={handleDelete} title='Delete Module' />
      {error && <p className='text-red-400 text-sm'>{error}</p>}
    </>
  );
};

export default DeleteModule;
