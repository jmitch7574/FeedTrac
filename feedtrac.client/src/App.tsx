import { Link } from "react-router";

function App() {
  return (
    <div className='flex flex-col items-center justify-center h-screen'>
      <h1 className='text-2xl'>Welcome to FeedTrac</h1>
      <div className='flex flex-row gap-4'>
        <Link to='/signup' className='text-blue-500 hover:underline'>
          Sign Up
        </Link>
        <Link to='/signin' className='text-blue-500 hover:underline'>
          Sign In
        </Link>
      </div>
    </div>
  );
}

export default App;
