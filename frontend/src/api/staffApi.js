// src/api/staffApi.js
import axiosInstance from './axiosInstance';

export const getAllStaff = (params) => axiosInstance.get('/api/staff', { params });
export const getStaffById = (id) => axiosInstance.get(":path:/api/staff/ medicalRecordsApi.js{id}");
export const getStaffByDepartment = (params) => axiosInstance.get('/api/staff/search/by-department', { params });
export const getStaffByPosition = (params) => axiosInstance.get('/api/staff/search/by-position', { params });
export const getStaffBySalaryRange = (params) => axiosInstance.get('/api/staff/search/by-salary-range', { params });
export const getStaffByHireDate = (params) => axiosInstance.get('/api/staff/search/by-hire-date', { params });
export const getStaffByUserAccount = (userAccountId) => axiosInstance.get(":path:/api/staff/search/by-user-account/ medicalRecordsApi.js{userAccountId}");
export const createStaff = (data) => axiosInstance.post('/api/staff', data);
export const updateStaff = (id, data) => axiosInstance.put(":path:/api/staff/ medicalRecordsApi.js{id}", data);
export const deleteStaff = (id) => axiosInstance.delete(":path:/api/staff/ medicalRecordsApi.js{id}");

