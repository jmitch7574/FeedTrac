import Logout from "@/components/auth/Logout";
import Welcome from "@/components/auth/Welcome";
import RenderModules from "@/components/modules/RenderModules";
import OptionsButton from "@/components/auth/OptionsButton.tsx";

const test = () => {
  return (
    <section className='h-screen bg-gray-100 flex flex-col'>
      <div className='flex flex-row items-center justify-between w-full bg-gray-200 py-4 px-6'>
        <div>{<Welcome />}</div>
        <div>
            <OptionsButton />
        </div>
        <div>
          <Logout />
        </div>
      </div>
      <div className='py-4 px-6 h-full '>
        <RenderModules />
      </div>
    </section>
  );
};

export default test;
