const API_BASE_URL = import.meta.env.VITE_FEEDTRAC_ENV_KEY;
import { AuthResponse, ForgotPasswordRequest, RefreshRequest, RefreshResponse, ResetPasswordRequest, studentLogin, studentRegister, teacherLogin, teacherRegister } from "@/types/Index";
import axios from "axios";


// reset password request
export const resetPasswordRequest = async (data: ResetPasswordRequest): Promise<void> => {
  // -- auth for reset password
  try {
    await axios.post(`${API_BASE_URL}/identity/resetPassword`, data, {
      withCredentials: true,
      headers: {
        "Content-Type": "application/json",
      },
    });
  } catch (error) {
    if (axios.isAxiosError(error) && error.response) {
      console.error("Error response:", error.response.data);
    } else {
      console.error("Error:", error);
    }
    throw error;
  }
};

// forgot password request - implemented here but not in the backend
export const forgotPasswordRequest = async (data: ForgotPasswordRequest): Promise<void> => {
  // -- forgot password request for any user
  try {
    await axios.post(`${API_BASE_URL}/identity/forgotPassword`, data, {
      withCredentials: true,
      headers: {
        "Content-Type": "application/json",
      },
    });
  } catch (error) {
    if (axios.isAxiosError(error) && error.response) {
      console.error("Error response:", error.response.data);
    } else {
      console.error("Error:", error);
    }
    throw error;
  }
};

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

// refresh token endpoint
export const refreshToken = async (data: RefreshRequest): Promise<RefreshResponse> => {
  const response = await axios.post(`${API_BASE_URL}/refresh`, data);
  return response.data; // Assuming API returns a new token
};
