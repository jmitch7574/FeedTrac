import { TicketMessage } from "@/types/Index";

interface MessageListProps {
  messages: TicketMessage[];
}

export default function MessageList({ messages }: MessageListProps) {
  return (
    <section className='space-y-4'>
      {messages.map((m) => (
        <div key={m.id} className='p-3 border rounded'>
          <p className='font-medium'>{m.senderName}</p>
          <p>{m.content}</p>
          <p className='text-xs text-gray-500'>{new Date(m.createdAt).toLocaleString()}</p>
        </div>
      ))}
    </section>
  );
}
