import { apiClient } from "@/lib/apiClient";
import { AuthResponse, studentLogin, studentRegister, teacherLogin, teacherRegister } from "@/types/Index";
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
