import RenderModules from "@/components/modules/RenderModules";
import RenderTickets from "../tickets/RenderTickets";
import Navigation from "../ui/Navigation";

const test = () => {
  return (
    <section className='h-screen bg-gray-100 flex flex-col'>
      <Navigation />

      <div className='py-4 px-6 h-full flex flex-col gap-4'>
        <RenderModules />
        <p className='text-2xl capitalize text-wrap'>Your Tickets:</p>
        <RenderTickets />
      </div>
    </section>
  );
};

export default test;
