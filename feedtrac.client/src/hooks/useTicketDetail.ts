import { useState, useEffect, useCallback } from "react";
import { getTicketById, addMessageToTicket, makeTicketResolved, summarizeTicket } from "@/hooks/useTickets";
import { Ticket } from "@/types/Index";

export default function useTicketDetail(ticketId: number) {
  const [ticket, setTicket] = useState<Ticket | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [summary, setSummary] = useState<string | null>(null);

  useEffect(() => {
    if (isNaN(ticketId)) {
      setError("Invalid ticket ID");
      setLoading(false);
      return;
    }

    let isMounted = true;
    (async () => {
      try {
        const t = await getTicketById(ticketId);
        if (isMounted) setTicket(t);
      } catch (err) {
        console.error(err);
        if (isMounted) setError("Could not load ticket.");
      } finally {
        if (isMounted) setLoading(false);
      }
    })();

    return () => {
      isMounted = false;
    };
  }, [ticketId]);

  const onAddMessage = useCallback(
    async (content: string) => {
      if (!ticket) return;
      const updated = await addMessageToTicket({ content, images: [] }, ticket.id);
      setTicket(updated);
    },
    [ticket]
  );

  const onResolve = useCallback(async () => {
    if (!ticket) return;
    const updated = await makeTicketResolved(ticket.id);
    setTicket(updated);
  }, [ticket]);

  const onSummarize = useCallback(async () => {
    const sum = await summarizeTicket(ticketId);
    setSummary(sum);
  }, [ticketId]);

  return { ticket, loading, error, summary, onAddMessage, onResolve, onSummarize };
}
