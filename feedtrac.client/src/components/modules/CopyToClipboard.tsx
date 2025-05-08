import { toast } from "sonner";
import { Button } from "@/components/ui/button";
import React from "react";
import { Copy } from "lucide-react";

const CopyToClipboard = ({ text }: { text: string }) => {

  const handleCopy = async (e: React.MouseEvent) => {
    e.stopPropagation();
    e.preventDefault();
    try {
      await navigator.clipboard.writeText(text);
      toast.success("Copied to clipboard!")
    } catch (err) {
      console.error("Failed to copy: ", err);
    }
  };

  return (
    <Button variant='outline' size='icon' onClick={handleCopy}>
      <Copy className='h-4 w-4' />
    </Button>
  );
};

export default CopyToClipboard;
