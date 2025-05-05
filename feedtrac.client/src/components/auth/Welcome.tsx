import { getUser } from "@/hooks/useUser";
import { useEffect, useState } from "react";

const Welcome = () => {
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    // if (!isAuthenticated) return; // 🔒 only fetch if logged in

    setLoading(true);

    getUser()
      .then((res) => {
        const user = res;
        setFirstName(user.firstName);
        setLastName(user.lastName);
        setLoading(false);
      })
      .catch(() => {
        setError("Failed to fetch user data");
        setLoading(false);
      });
  }, []);

  if (loading) return <div>Loading...</div>;
  if (error) return <div>{error}</div>;

  return (
    <div className='font-semibold text-2xl text-gray-800'>
      <span className='s'>Welcome</span>, {firstName} {lastName}!
    </div>
  );
};

export default Welcome;
