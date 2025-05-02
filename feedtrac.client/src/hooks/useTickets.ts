import { apiClient } from "@/lib/apiClient";
import { AddMessageFormData, CreateTicketFormData, Ticket } from "@/types/Index";
import axios from "axios";

// Ticket related endpoints
export const getTicketBySignedInUser = async (): Promise<Ticket> => {
  // -- get all tickets for user
  try {
    const response = await apiClient.get("/tickets");
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

export const getTicketById = async (id: number): Promise<Ticket> => {
  // -- get ticket by id
  try {
    const response = await apiClient.get(`/tickets/${id}`);
    return response.data.ticket;
  } catch (error) {
    if (axios.isAxiosError(error) && error.response) {
      console.error("Error response:", error.response.data);
    } else {
      console.error("Error:", error);
    }
    throw error;
  }
};

export const createTicket = async (data: CreateTicketFormData, moduleId: number): Promise<Ticket> => {
  // -- create ticket
  const formData = new FormData();
  formData.append("Title", data.title);
  formData.append("FirstMessage.Content", data.content);
  data.images?.forEach((img) => formData.append("FirstMessage.Images", img));

  try {
    const response = await apiClient.post(`/tickets/${moduleId}/create`, formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });

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

// adds a message to update the ticket
export const addMessageToTicket = async (data: AddMessageFormData, ticketId: number): Promise<Ticket> => {
  const formData = new FormData();
  formData.append("Content", data.content);
  data.images?.forEach((img) => formData.append("Images", img));

  // -- create ticket
  try {
    const response = await apiClient.post(`/tickets/${ticketId}/addMessage`, formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
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

// mark ticket as resolved
export const makeTicketResolved = async (ticketId: number): Promise<Ticket> => {
  // -- create ticket
  try {
    const response = await apiClient.post(`/tickets/${ticketId}/markAsResolved`);
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

// getting images for a ticket

export const getTicketImage = async (imageId: number): Promise<Blob> => {
  // -- get ticket image by imageId
  try {
    const response = await apiClient.get(`/tickets/${imageId}`, {
      responseType: "blob", // Set the response type to 'blob' for image data
    });
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
