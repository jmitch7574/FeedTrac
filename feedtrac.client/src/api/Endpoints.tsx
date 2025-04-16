const API_BASE_URL = import.meta.env.VITE_FEEDTRAC_ENV_KEY;
import { AuthResponse, ForgotPasswordRequest, RefreshRequest, RefreshResponse, ResetPasswordRequest, studentLogin, studentRegister } from "@/types/Index";
import axios from "axios";

// auth for students
export const registerUser = async (data: studentRegister): Promise<AuthResponse> => {
  try {
    const response = await axios.post(`${API_BASE_URL}/student/register`, data);
    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error) && error.response) {
      console.error("Error response:", error.response.data);
    } else {
      console.error("Error:", error);
    }
    throw error;
  }
};

export const loginUser = async (data: studentLogin): Promise<AuthResponse> => {
  try {
    const response = await axios.post(`${API_BASE_URL}/student/login`, data, {
      withCredentials: true, // 👈 this is the key!
      headers: {
        "Content-Type": "application/json",
      },
    });
    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error) && error.response) {
      console.error("❌ Error response:", error.response.data);
    } else {
      console.error("❌ Error:", error);
    }
    throw error;
  }
};

// fetch("http://localhost:5135/user", {
//   method: "GET",
//   credentials: "include", // Include cookies in the request
// })
//   .then((response) => response.json())
//   .then((data) => console.log(data))
//   .catch((error) => console.error("Error:", error));

const testFetchUser = async () => {
  try {
    const response = await fetch("http://localhost:5135/user", {
      method: "GET",
      credentials: "include",
      headers: {
        "Content-Type": "application/json",
      },
    });

    const contentType = response.headers.get("content-type");

    if (!response.ok) {
      const text = await response.text(); // Read raw text for debugging
      console.error("❌ Server Error:", text);
      throw new Error("Failed to fetch user");
    }

    if (contentType && contentType.includes("application/json")) {
      const data = await response.json();
      console.log("✅ User Data:", data);
    } else {
      const text = await response.text();
      console.warn("⚠️ Non-JSON response:", text);
    }
  } catch (err) {
    console.error("❌ Error:", err.message);
  }
};
testFetchUser();

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

// password related endpoints
export const forgotPassword = async (data: ForgotPasswordRequest): Promise<void> => {
  await axios.post(`${API_BASE_URL}/forgotPassword`, data);
};

export const resetPassword = async (data: ResetPasswordRequest): Promise<void> => {
  return axios.post(`${API_BASE_URL}/resetPassword`, data);
};
