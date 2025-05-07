import { useEffect, useState } from "react";
import { Card } from "@/components/ui/card.tsx";
import { getTicketSummary } from "@/hooks/useTickets";
import { Bot } from "lucide-react";

interface AiSummaryProps {
  ticketId: number;
}

const AiSummary = ({ ticketId }: AiSummaryProps) => {
  const [text, setText] = useState("Loading...");

  useEffect(() => {
    const getSummary = async () => {
      const summary: string = await getTicketSummary(ticketId);
      setText(summary);
    };
    getSummary();
  }, []);

  return (
    <div>
      <Card className='flex rounded-3xl flex-col'>
        <div className='px-5 p py-3 bg-gray-600 font-semibold text-2xl flex items-center'>
          <Bot className='mx-2 text-gray-200' />
          <p className='text-gray-200'>FeedTrac AI Assistant</p>
        </div>
        <div className='px-5 py-1'>
          <p>{text}</p>
        </div>
      </Card>
    </div>
  );
};

export default AiSummary;
