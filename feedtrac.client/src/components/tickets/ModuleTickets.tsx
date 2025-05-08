// src/components/ModuleTickets.tsx
import { useEffect, useState } from "react";
import { getTicketsByModuleId } from "@/hooks/useTickets";
import { Ticket } from "@/types/Index";
import { TicketCard } from "./TicketCard";

interface ModuleTicketsProps {
  moduleId: number;
  refresh: Boolean;
}

export default function ModuleTickets({ moduleId, refresh }: ModuleTicketsProps) {
  const [tickets, setTickets] = useState<Ticket[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetch = async () => {
      try {
        const data = await getTicketsByModuleId(moduleId);
        setTickets(data);
      } catch (err) {
        console.error(err);
        setError("Could not load tickets");
      } finally {
        setLoading(false);
      }
    };
    fetch();
  }, [moduleId, refresh]);

  if (loading) return <p>Loading tickets…</p>;
  if (error) return <p className='text-red-500'>{error}</p>;
  if (tickets.length === 0) return <p>No tickets for this module.</p>;

  return (
    <ul className='space-y-4 flex flex-wrap gap-4 flex-row'>
      {tickets.map((ticket) => (
        <TicketCard key={ticket.id} ticket={ticket} />
      ))}
    </ul>
  );
}
