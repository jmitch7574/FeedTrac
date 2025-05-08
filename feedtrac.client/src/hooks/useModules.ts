import { apiClient } from "@/lib/apiClient";
import { Module, ModuleResponse } from "@/types/Index";
import axios from "axios";

// modules related endpoints
export const getAllModulesForUser = async (): Promise<ModuleResponse> => {
  // -- get all modules for user
  try {
    const response = await apiClient.get("/modules");
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

export const getAllModules = async (): Promise<ModuleResponse> => {
  // -- get all modules
  try {
    const response = await apiClient.get("/modules/all");
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

export const joinModule = async (joinCode: string): Promise<Module> => {
  // -- join module
  try {
    const response = await apiClient.post(`/modules/join?joinCode=${joinCode}`);
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

export const getModuleInfo = async (id: number): Promise<ModuleResponse> => {
  // -- get Module Infomation by id
  try {
    const response = await apiClient.get(`/modules/${id}`);
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

export const deleteModule = async (id: number): Promise<void> => {
  // -- delete Module by id
  try {
    await apiClient.delete(`/modules/${id}`);
  } catch (error) {
    if (axios.isAxiosError(error) && error.response) {
      console.error("Error response:", error.response.data);
    } else {
      console.error("Error:", error);
    }
    throw error;
  }
};

export const studentLeaveModule = async (id: number): Promise<void> => {
  // -- student leave module by id
  try {
    await apiClient.delete(`/modules/${id}/leave`);
  } catch (error) {
    if (axios.isAxiosError(error) && error.response) {
      console.error("Error response:", error.response.data);
    } else {
      console.error("Error:", error);
    }
    throw error;
  }
};

// export const createModule = async (data: moduleName): Promise<Module> => {
//   // -- create a module using name
//   try {
//     const response = await apiClient.post(`/create`, data);
//     return response.data;
//   } catch (error) {
//     if (axios.isAxiosError(error) && error.response) {
//       console.error("Error response:", error.response.data);
//     } else {
//       console.error("Error:", error);
//     }
//     throw error;
//   }
// };

export const createModule = async (name: string): Promise<Module> => {
  try {
    const response = await apiClient.post(`/create?name=${encodeURIComponent(name)}`);
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
