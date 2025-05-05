import { apiClient } from "@/lib/apiClient";
import { ModuleResponse, PublicUser } from "@/types/Index";
import axios from "axios";

export const getUser = async (): Promise<PublicUser> => {
  // -- get user data
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

export const getUserModules = async (): Promise<ModuleResponse> => {
  // -- get user modules
  try {
    const response = await apiClient.get("/user/modules");
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
