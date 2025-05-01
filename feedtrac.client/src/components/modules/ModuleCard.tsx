import { Link } from "react-router";

const ModuleCard = ({ moduleName }: { moduleName: string }) => {
  return (
    <Link to={`/module/${moduleName}`} className='h-[192px] w-[328px] bg-gray-300 rounded flex justify-center items-center text-2xl outline-dashed outline-2 outline-gray-400 hover:bg-gray-400 transition duration-200 ease-in-out'>
      <p>{moduleName}</p>
    </Link>
  );
};

{
  /* <div className='h-[192px] w-[328px] bg-gray-300 rounded flex justify-center items-center text-2xl outline-dashed outline-2 outline-gray-400'>{moduleName}</div>; */
}
export default ModuleCard;
