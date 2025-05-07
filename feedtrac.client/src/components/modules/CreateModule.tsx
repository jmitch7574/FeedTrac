import { Dialog, DialogTrigger, DialogContent, DialogHeader, DialogTitle, DialogFooter } from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { createModule } from "@/hooks/useModules";
import { useState } from "react";
import { Module } from "@/types/Index";
import { toast } from "sonner";

const CreateModule = ({ onModuleCreated }: { onModuleCreated: (newModule: Module) => void }) => {
  const [moduleName, setModuleName] = useState("");
  const [open, setOpen] = useState(false);

  const handleSubmit = async () => {
    try {
      const res = await createModule(moduleName);
      onModuleCreated(res);
      toast.success("Module created successfully!");
      setOpen(false); // close the dialog
      setModuleName(""); // reset input
    } catch {
      toast.error("Failed to create module. Please try again.");
    }
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <button className='capitalize h-[192px] w-[328px] bg-gray-300 rounded flex justify-center items-center outline-dashed outline-2 outline-gray-400 hover:bg-gray-400 transition duration-200 ease-in-out'>
          <p className='text-xl'>create a module</p>
        </button>
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Create New Module</DialogTitle>
        </DialogHeader>
        <div className='space-y-4'>
          <Input placeholder='Module name' value={moduleName} onChange={(e) => setModuleName(e.target.value)} />
        </div>
        <DialogFooter>
          <Button onClick={handleSubmit}>Create</Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};

export default CreateModule;
