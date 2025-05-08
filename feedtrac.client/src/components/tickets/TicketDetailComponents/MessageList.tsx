import { TicketMessage } from "@/types/Index";
const API_BASE_URL = import.meta.env.VITE_FEEDTRAC_ENV_KEY;

interface MessageListProps {
  messages: TicketMessage[];
}

export default function MessageList({ messages }: MessageListProps) {
  return (
    <section className='space-y-4'>
      {messages.map((m) => (
        <div key={m.id} className='p-3 border shadow-md rounded'>
          <p className='font-medium'>{m.senderName}</p>
          <p>{m.content}</p>
          <div className='flex flex-row m-5'>
            {
              m.imageIds.map((m) => (
                  <a href={`${API_BASE_URL}/image/${m}`} target="_blank" rel="noopener noreferrer">
                    <img className="h-96 object-cover rounded" src={`${API_BASE_URL}/image/${m}`} alt="MessageImage"/>
                  </a>
              ))
            }
          </div>
          <p className='text-xs text-gray-500'>{new Date(m.createdAt).toLocaleString()}</p>
        </div>
      ))}
    </section>
  );
}
