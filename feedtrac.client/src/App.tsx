import { Link } from "react-router";
import { getUser } from "./hooks/useUser";
import { useEffect, useState } from "react";

function App() {
  const [firstName, setFirstName] = useState<string | null>(null);
  const [lastName, setLastName] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    getUser()
      .then((user) => {
        setFirstName(user.firstName);
        setLastName(user.lastName); // Set last name as well
        setLoading(false);
      })
      .catch((err) => {
        setError("Failed to fetch user data");
        setLoading(false);
      });
  }, []); // Empty dependency array to run this effect once on mount

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error}</div>;
  }

  return (
    <div className='flex flex-col items-center justify-center h-screen'>
      <h1>
        Welcome, {firstName ? firstName : "Guest"} {lastName ? lastName : ""}
      </h1>
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
