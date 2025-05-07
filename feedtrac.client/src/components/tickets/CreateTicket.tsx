import { Dialog, DialogTrigger, DialogContent, DialogHeader, DialogTitle, DialogFooter } from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Textarea } from "@/components/ui/textarea";
import { useState } from "react";
import { toast } from "sonner";
import { createTicket } from "@/hooks/useTickets";
import { Ticket } from "@/types/Index";

interface CreateTicketProps {
  moduleId: number;
  onTicketCreated: (newTicket: Ticket) => void;
}

const CreateTicket = ({ moduleId, onTicketCreated }: CreateTicketProps) => {
  const [title, setTitle] = useState("");
  const [content, setContent] = useState("");
  const [images, setImages] = useState<File[]>([]);
  const [open, setOpen] = useState(false);

  const handleImageUpload = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files) {
      setImages(Array.from(e.target.files));
    }
  };

  const handleSubmit = async () => {
    if (!title || !content) {
      toast.error("Title and content are required.");
      return;
    }

    try {
      const res = await createTicket({ title, content, images }, moduleId);
      onTicketCreated(res);
      toast.success("Ticket created successfully!");
      setOpen(false);
      setTitle("");
      setContent("");
      setImages([]);
    } catch {
      toast.error("Failed to create ticket. Please try again.");
    }
  };

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <button className='capitalize h-[192px] w-[328px] bg-gray-300 rounded flex justify-center items-center outline-dashed outline-2 outline-gray-400 hover:bg-gray-400 transition duration-200 ease-in-out'>
          <p className='text-xl'>Create a Ticket</p>
        </button>
      </DialogTrigger>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Create New Ticket</DialogTitle>
        </DialogHeader>
        <div className='space-y-4'>
          <Input placeholder='Title' value={title} onChange={(e) => setTitle(e.target.value)} />
          <Textarea placeholder='Content' value={content} onChange={(e) => setContent(e.target.value)} />
          <Input type='file' multiple onChange={handleImageUpload} />
        </div>
        <DialogFooter>
          <Button onClick={handleSubmit}>Create</Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};

export default CreateTicket;
