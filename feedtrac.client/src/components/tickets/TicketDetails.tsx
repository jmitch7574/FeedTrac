// src/components/tickets/TicketDetail.tsx
import { useParams } from "react-router-dom";
// import SummarySection from "./SummarySection";  // temporarily disabled
import useTicketDetail from "@/hooks/useTicketDetail";
import TicketHeader from "./TicketDetailComponents/TicketHeader";
import MessageList from "./TicketDetailComponents/MessageList";
import AddMessageForm from "./TicketDetailComponents/AddMessageForm";

export default function TicketDetail() {
  const { ticketId } = useParams<{ ticketId: string }>();
  const id = ticketId ? Number(ticketId) : NaN;
  const { ticket, loading, error, /* summary, */ onAddMessage, onResolve /* onSummarize */ } = useTicketDetail(id);

  // Debug logging
  console.log("TicketDetail render:", { ticketId, id, loading, error, ticket });

  if (loading) return <p>Loading ticket…</p>;
  if (error) return <p className='text-red-500'>Error: {error}</p>;
  if (!ticket) return <p>Ticket not found.</p>;

  return (
    <div className='space-y-6'>
      <TicketHeader ticket={ticket} onResolve={onResolve} />
      <MessageList messages={ticket.messages} />
      <AddMessageForm onAdd={onAddMessage} />
      {/* <SummarySection summary={summary} onSummarize={onSummarize} /> */}
    </div>
  );
}
