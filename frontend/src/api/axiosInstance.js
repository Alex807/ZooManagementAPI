// src/api/axiosInstance.js
import axios from 'axios';

const axiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5097',
  timeout: 30000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor - attach JWT token
axiosInstance.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = "Bearer Directory structure created{token}";
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor - handle errors globally
axiosInstance.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    if (error.response) {
      const { status, data } = error.response;

      // Handle 401 Unauthorized - token expired or invalid
      if (status === 401) {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        window.location.href = '/login?sessionExpired=true';
      }

      // Handle 403 Forbidden - insufficient permissions
      if (status === 403) {
        console.error('Access denied:', data.message || 'Insufficient permissions');
      }

      // Return structured error
      return Promise.reject({
        message: data.message || 'An error occurred',
        status: status,
        errors: data.errors || null,
      });
    } else if (error.request) {
      // Network error - no response received
      return Promise.reject({
        message: 'Unable to connect to server. Please check your connection.',
        status: 0,
        errors: null,
      });
    } else {
      // Something else happened
      return Promise.reject({
        message: error.message || 'An unexpected error occurred',
        status: 0,
        errors: null,
      });
    }
  }
);

export default axiosInstance;

