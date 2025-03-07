const APIKey = import.meta.env.VITE_API_KEY;
import { AuthResponse, ForgotPasswordRequest, RefreshRequest, RefreshResponse, ResetPasswordRequest, studentLogin, studentRegister } from "@/types";
import axios from "axios";

// auth for students
// register endpoint for students
export const registerUser = async (data: studentRegister): Promise<AuthResponse> => {
  const response = await axios.post(`${APIKey}/student/register`, data);
  return response.data;
};

// login endpoint for students
export const loginUser = async (data: studentLogin): Promise<AuthResponse> => {
  const response = await axios.post(`${APIKey}/student/login`, data);
  return response.data;
};

// // logout endpoint for students
// export const logoutUser = async (data: any) => {
//   const response = await axios.post(`${APIKey}/student/logout`, data);
//   return response.data;
// };

// refresh token endpoint
export const refreshToken = async (data: RefreshRequest): Promise<RefreshResponse> => {
  const response = await axios.post(`${APIKey}/refresh`, data);
  return response.data; // Assuming API returns a new token
};

// password related endpoints
export const forgotPassword = async (data: ForgotPasswordRequest): Promise<void> => {
  await axios.post(`${APIKey}/forgotPassword`, data);
};

export const resetPassword = async (data: ResetPasswordRequest): Promise<void> => {
  return axios.post(`${APIKey}/resetPassword`, data);
};
