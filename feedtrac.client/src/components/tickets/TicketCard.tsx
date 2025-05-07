import { Ticket } from "@/types/Index";
import { Link } from "react-router";

export const TicketCard: React.FC<{ ticket: Ticket }> = ({ ticket }) => (
  <Link to={`/tickets/${ticket.id}`} className='relative h-[192px] w-[328px] text-black bg-gray-300 rounded flex flex-col justify-center items-center hover:bg-gray-400 transition duration-200 ease-in-out'>
    <h3 className='font-bold capitalize text-wrap'>{ticket.title}</h3>
    <p>Status: {ticket.status === 1 ? "Resolved" : "Open"}</p>
    <small>{new Date(ticket.createdAt).toLocaleString()}</small>
  </Link>
);
