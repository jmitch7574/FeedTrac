import OptionsButton from "@/components/auth/OptionsButton.tsx";

import Logout from "@/components/auth/Logout";
import Welcome from "@/components/auth/Welcome";

const Navigation = () => {
  return (
    <div className='flex flex-row items-center justify-between w-full bg-gray-200 py-4 px-6'>
      <div>{<Welcome />}</div>
      <div>
        <OptionsButton />
      </div>
      <div>
        <Logout />
      </div>
    </div>
  );
};

export default Navigation;
