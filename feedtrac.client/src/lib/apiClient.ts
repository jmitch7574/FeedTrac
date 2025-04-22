import axios from "axios";
const API_BASE_URL = import.meta.env.VITE_FEEDTRAC_ENV_KEY;

if (!API_BASE_URL) {
  throw new Error("VITE_FEEDTRAC_ENV_KEY is not defined");
}

export const apiClient = axios.create({
  baseURL: `${API_BASE_URL}`,
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
  },
});
