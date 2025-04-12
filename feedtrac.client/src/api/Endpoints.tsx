const API_BASE_URL = import.meta.env.VITE_FEEDTRAC_ENV_KEY;
import { AuthResponse, ForgotPasswordRequest, RefreshRequest, RefreshResponse, ResetPasswordRequest, studentLogin, studentRegister } from "@/types";
import axios from "axios";

// auth for students
export const registerUser = async (data: studentRegister): Promise<AuthResponse> => {
  try {
    const response = await axios.post(`${API_BASE_URL}/student/register`, data);
    return response.data;
  } catch (error) {
    if (error.response) {
      console.error("Error response:", error.response.data);
    } else {
      console.error("Error:", error);
    }
    throw error;
  }
};

// login endpoint for students
export const loginUser = async (data: studentLogin): Promise<AuthResponse> => {
  const response = await axios.post(`${API_BASE_URL}/student/login`, data);
  return response.data;
};

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
