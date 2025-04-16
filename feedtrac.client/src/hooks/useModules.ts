import { apiClient } from "@/lib/apiClient";
import { Module, moduleName, ModuleResponse, teacherModuleEmail, teacherModuleId } from "@/types/Index";
import axios from "axios";

// modules related endpoints
export const getAllModulesUser = async (): Promise<ModuleResponse> => {
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

export const joinModule = async (data: Module): Promise<ModuleResponse> => {
  // -- join module
  try {
    const response = await apiClient.post("/modules/join", data);
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

export const assignTeacherToModuleById = async (moduleId: string, data: teacherModuleId): Promise<ModuleResponse> => {
  // -- assign teacher to module
  try {
    const response = await apiClient.post(`/${moduleId}/assignTeacher/byTeacherId`, data);
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

export const assignTeacherToModuleByEmail = async (moduleId: string, data: teacherModuleEmail): Promise<ModuleResponse> => {
  // -- assign teacher to module by email
  try {
    const response = await apiClient.post(`/${moduleId}/assignTeacher/byTeacherEmail`, data);
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

export const deleteModule = async (id: number): Promise<ModuleResponse> => {
  // -- delete Module by id
  try {
    const response = await apiClient.delete(`/modules/${id}`);
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

export const createModule = async (data: moduleName): Promise<ModuleResponse> => {
  // -- create a module using name
  try {
    const response = await apiClient.post(`/create`, data);
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
