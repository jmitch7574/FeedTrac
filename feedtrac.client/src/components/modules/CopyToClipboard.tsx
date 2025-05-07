import { toast } from "sonner";
import { Button } from "@/components/ui/button";
import React, { useState } from "react";
import { Copy } from "lucide-react";

const CopyToClipboard = ({ text }: { text: string }) => {
  const [copied, setCopied] = useState(false);

  const handleCopy = async (e: React.MouseEvent) => {
    e.stopPropagation();
    e.preventDefault();
    try {
      await navigator.clipboard.writeText(text);
      setCopied(true);
      setTimeout(() => setCopied(false), 2000); // Reset after 2s
    } catch (err) {
      console.error("Failed to copy: ", err);
    }
  };

  return (
    <Button variant='outline' size='icon' onClick={handleCopy}>
      <Copy className='h-4 w-4' />
      <span className='sr-only'>Copy</span>
      {copied && (
        <>
          {toast.success("Copied to clipboard!")}
          {/* <span className='ml-2 text-sm text-green-500'>Copied!</span> */}
        </>
      )}
    </Button>
  );
};

export default CopyToClipboard;
