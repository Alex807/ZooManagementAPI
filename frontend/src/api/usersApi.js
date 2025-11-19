// src/api/usersApi.js
import axiosInstance from './axiosInstance';

export const getAllUsers = (params) => axiosInstance.get('/api/users', { params });
export const getUserById = (id) => axiosInstance.get(":path:/api/users/ authApi.js{id}");
export const getCurrentUser = () => axiosInstance.get('/api/users/me');
export const getUserByUsername = (username) => axiosInstance.get(":path:/api/users/search/username/ authApi.js{username}");
export const getUserByEmail = (email) => axiosInstance.get(":path:/api/users/search/email/ authApi.js{email}");
export const getUsersByRole = (roleId) => axiosInstance.get(":path:/api/users/search/role/ authApi.js{roleId}");
export const updateUser = (id, data) => axiosInstance.put(":path:/api/users/ authApi.js{id}", data);
export const changeUserRole = (userId, data) => axiosInstance.put(":path:/api/users/ authApi.js{userId}/change-role", data);
export const deleteUser = (id) => axiosInstance.delete(":path:/api/users/ authApi.js{id}");
export const assignRoleToUser = (userId, roleId) => axiosInstance.post(":path:/api/users/ authApi.js{userId}/roles/ authApi.js{roleId}");
export const removeRoleFromUser = (userId, roleId) => axiosInstance.delete(":path:/api/users/ authApi.js{userId}/roles/ authApi.js{roleId}");

