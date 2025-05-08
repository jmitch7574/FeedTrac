import { useState } from "react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { toast } from "sonner";

interface AddMessageFormProps {
  onAdd: (content: string, images: File[]) => Promise<void>;
}

export default function AddMessageForm({ onAdd }: AddMessageFormProps) {
  const [content, setContent] = useState("");
  const [images, setImages] = useState<File[]>([]);


  const handleImageUpload = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files) {
      setImages(Array.from(e.target.files));
    }
  };
  const handleSubmit = async () => {
    if (!content.trim()) {
      toast.error("Message cannot be empty");
      return;
    }
    try {
      await onAdd(content, images);
      setContent("");
      setImages([]);
    } catch {
      toast.error("Failed to send message");
    }
  };

  return (
    <section className='space-y-3 p-5 bg-gray-600 rounded-3xl'>
      <Input className='bg-white' placeholder='Type your message...' value={content} onChange={(e) => setContent(e.target.value)} />
      <Input className='bg-white max-w-54' type='file' multiple onChange={handleImageUpload} />
      <Button className='bg-black' onClick={handleSubmit} disabled={!content.trim()}>
        Send Message
      </Button>
    </section>
  );
}
