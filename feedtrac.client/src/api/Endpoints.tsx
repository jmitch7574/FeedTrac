// const testFetchUser = async () => {
//   try {
//     const response = await fetch("http://localhost:5135/user", {
//       method: "GET",
//       credentials: "include",
//       headers: {
//         "Content-Type": "application/json",
//       },
//     });

//     const contentType = response.headers.get("content-type");

//     if (!response.ok) {
//       const text = await response.text(); // Read raw text for debugging
//       console.error("❌ Server Error:", text);
//       throw new Error("Failed to fetch user");
//     }

//     if (contentType && contentType.includes("application/json")) {
//       const data = await response.json();
//       console.log("✅ User Data:", data);
//     } else {
//       const text = await response.text();
//       console.warn("⚠️ Non-JSON response:", text);
//     }
//   } catch (err) {
//     console.error("❌ Error:", err.message);
//   }
// };
// testFetchUser();

// // logout endpoint for students
// export const logoutUser = async (data: any) => {
//   const response = await axios.post(`${APIKey}/student/logout`, data);
//   return response.data;
// };
