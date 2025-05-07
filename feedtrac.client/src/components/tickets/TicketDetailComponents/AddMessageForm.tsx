import { useState } from "react";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { toast } from "sonner";

interface AddMessageFormProps {
  onAdd: (content: string) => Promise<void>;
}

export default function AddMessageForm({ onAdd }: AddMessageFormProps) {
  const [content, setContent] = useState("");

  const handleSubmit = async () => {
    if (!content.trim()) {
      toast.error("Message cannot be empty");
      return;
    }
    try {
      await onAdd(content);
      setContent("");
    } catch {
      toast.error("Failed to send message");
    }
  };

  return (
    <section className='space-y-3'>
      <Input placeholder='Type your message...' value={content} onChange={(e) => setContent(e.target.value)} />
      <Button onClick={handleSubmit} disabled={!content.trim()}>
        Send Message
      </Button>
    </section>
  );
}
