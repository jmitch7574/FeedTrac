import { Ticket } from "@/types/Index";
import { Button } from "@/components/ui/button";

interface TicketHeaderProps {
  ticket: Ticket;
  onResolve: () => Promise<void>;
}

export default function TicketHeader({ ticket, onResolve }: TicketHeaderProps) {
  return (
    <header className='flex items-center justify-between'>
      <h1 className='text-2xl font-bold'>{ticket.title}</h1>
      {ticket.status !== 2 ? <Button onClick={onResolve}>Mark Resolved</Button> : <span className='text-green-600'>Resolved</span>}
    </header>
  );
}
