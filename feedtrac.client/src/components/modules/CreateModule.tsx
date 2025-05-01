import { createModule } from "@/hooks/useModules";

const CreateModule = () => {
  const handleClick = async () => {
    try {
      const newModule = { moduleName: "software engineering" };
      const res = await createModule(newModule);
      console.log("Module created:", res.modules);
    } catch (error) {
      console.error("Error creating module:", error);
    }

    console.log("Create a new module");
  };

  return (
    <button onClick={handleClick} className='h-[192px] w-[328px] bg-gray-300 rounded flex justify-center items-center text-2xl outline-dashed outline-2 outline-gray-400 hover:bg-gray-400 transition duration-200 ease-in-out'>
      <p>create a module</p>
    </button>
  );
};

export default CreateModule;
