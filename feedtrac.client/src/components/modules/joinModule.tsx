import { Dialog, DialogTrigger, DialogContent, DialogHeader, DialogTitle, DialogFooter } from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { joinModule } from "@/hooks/useModules"; // Adjust the import if needed
import { useState } from "react";

const JoinModule = ({ onModuleJoined }: { onModuleJoined: () => void }) => {
  const [joinCode, setJoinCode] = useState("");
  const [open, setOpen] = useState(false);

  const handleSubmit = async () => {
    try {
      await joinModule(joinCode); // Call joinModule with the joinCode
      onModuleJoined(); // Pass the result to the callback
      setOpen(false); // Close the dialog
      setJoinCode(""); // Reset input field
    } catch (error) {
      console.error("Error joining module:", error);
    }
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <button className='capitalize h-[192px] w-[328px] bg-gray-300 rounded flex justify-center items-center outline-dashed outline-2 outline-gray-400 hover:bg-gray-400 transition duration-200 ease-in-out'>
          <p className='text-xl'>Join a module</p>
        </button>
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Join Module</DialogTitle>
        </DialogHeader>
        <div className='space-y-4'>
          <Input placeholder='Enter join code' value={joinCode} onChange={(e) => setJoinCode(e.target.value)} />
        </div>
        <DialogFooter>
          <Button onClick={handleSubmit}>Join</Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};

export default JoinModule;
