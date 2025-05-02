import { apiClient } from "@/lib/apiClient";
import { AuthResponse, ForgotPasswordRequest, IdentityResponse, ResetPasswordRequest, studentLogin, studentRegister, teacherLogin, teacherRegister } from "@/types/Index";
import axios from "axios";

// auth for students
export const registerStudent = async (data: studentRegister): Promise<AuthResponse> => {
  // -- auth for register students
  try {
    const response = await apiClient.post("/identity/student/register", data);
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

export const loginStudent = async (data: studentLogin): Promise<AuthResponse> => {
  // -- auth for login students
  try {
    const response = await apiClient.post("/identity/student/login", data);
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

// auth for teachers
export const registerTeacher = async (data: teacherRegister): Promise<AuthResponse> => {
  // -- auth for registering teachers
  try {
    const response = await apiClient.post("/identity/teacher/register", data);
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

export const loginTeacher = async (data: teacherLogin): Promise<AuthResponse> => {
  // -- auth for login teachers
  try {
    const response = await apiClient.post("/identity/teacher/login", data);
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

// reset password request
export const resetPasswordRequest = async (data: ResetPasswordRequest): Promise<void> => {
  // -- auth for reset password
  try {
    await apiClient.post("/identity/resetPassword", data);
  } catch (error) {
    if (axios.isAxiosError(error) && error.response) {
      console.error("Error response:", error.response.data);
    } else {
      console.error("Error:", error);
    }
    throw error;
  }
};

// forgot password request
export const forgotPasswordRequest = async (data: ForgotPasswordRequest): Promise<void> => {
  // -- forgot password request for any user
  try {
    await apiClient.post("/identity/forgotPassword/request", data);
  } catch (error) {
    if (axios.isAxiosError(error) && error.response) {
      console.error("Error response:", error.response.data);
    } else {
      console.error("Error:", error);
    }
    throw error;
  }
};

// forgot password followup
export const forgotPasswordRequestFollowUp = async (data: ResetPasswordRequest): Promise<void> => {
  // -- forgot password request for any user
  try {
    await apiClient.post("/identity/forgotPassword/followup", data);
  } catch (error) {
    if (axios.isAxiosError(error) && error.response) {
      console.error("Error response:", error.response.data);
    } else {
      console.error("Error:", error);
    }
    throw error;
  }
};

// logout point
export const userLogout = async (): Promise<void> => {
  // -- forgot password request for any user
  try {
    await apiClient.post("/identity/logout");
  } catch (error) {
    if (axios.isAxiosError(error) && error.response) {
      console.error("Error response:", error.response.data);
    } else {
      console.error("Error:", error);
    }
    throw error;
  }
};

// get user info
export const getUserInfo = async (): Promise<IdentityResponse> => {
  // -- get user info
  try {
    const response = await apiClient.get("/identity");
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
