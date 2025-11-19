// src/api/authApi.js
import axiosInstance from './axiosInstance';

export const register = (data) => {
  return axiosInstance.post('/api/auth/register', data);
};

export const login = (data) => {
  return axiosInstance.post('/api/auth/login', data);
};

export const changePassword = (data) => {
  return axiosInstance.post('/api/auth/change-password', data);
};

export const refreshToken = () => {
  return axiosInstance.post('/api/auth/refresh-token');
};
