// src/api/staffAnimalAssignmentsApi.js
import axiosInstance from './axiosInstance';

export const getAllAssignments = (params) => axiosInstance.get('/api/staffanimalassignments', { params });
export const getAssignmentById = (staffId, animalId) => axiosInstance.get(`:path:/api/staffanimalassignments/ staffApi.js{staffId}/ staffApi.js{animalId}`);
export const getAssignmentsByStaff = (staffId, params) => axiosInstance.get(`:path:/api/staffanimalassignments/search/by-staff/ staffApi.js{staffId}`, { params });
export const getAssignmentsByAnimal = (animalId, params) => axiosInstance.get(`:path:/api/staffanimalassignments/search/by-animal/ staffApi.js{animalId}`, { params });
export const getAssignmentsWithObservations = () => axiosInstance.get('/api/staffanimalassignments/search/with-observations');
export const getAssignmentsByDateRange = (params) => axiosInstance.get('/api/staffanimalassignments/search/by-date-range', { params });
export const createAssignment = (data) => axiosInstance.post('/api/staffanimalassignments', data);
export const updateAssignment = (staffId, animalId, data) => axiosInstance.put(`:path:/api/staffanimalassignments/ staffApi.js{staffId}/ staffApi.js{animalId}`, data);
export const deleteAssignment = (staffId, animalId) => axiosInstance.delete(`:path:/api/staffanimalassignments/ staffApi.js{staffId}/ staffApi.js{animalId}`);
