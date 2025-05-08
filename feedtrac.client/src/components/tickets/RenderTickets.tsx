// components/RenderTickets.tsx
import { useEffect, useState } from "react";
import { getTicketBySignedInUser } from "@/hooks/useTickets";
import { Ticket, TicketResponse } from "@/types/Index";
import { TicketCard } from "./TicketCard";

export default function RenderTickets() {
  const [tickets, setTickets] = useState<Ticket[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchTickets = async () => {
      setLoading(true);
      try {
        // call the hook, which returns { tickets: Ticket[] }
        const res: TicketResponse = await getTicketBySignedInUser();
        setTickets(res);
      } catch (err) {
        console.error("Failed to fetch tickets:", err);
        setError("Failed to load tickets");
      } finally {
        setLoading(false);
      }
    };

    fetchTickets();
  }, []);

  if (loading) {
    return <p>Loading tickets…</p>;
  }

  if (error) {
    return <p className='text-red-500'>{error}</p>;
  }

  if (!tickets || tickets.length === 0) {
    return <p>No tickets available.</p>;
  }

  return (
    <div className='flex flex-wrap gap-4'>
      {tickets.map((ticket) => (
        <TicketCard key={ticket.id} {...ticket} ticket={ticket} />
      ))}
    </div>
  );
}
