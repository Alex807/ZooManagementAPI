// src/api/medicalRecordsApi.js
import axiosInstance from './axiosInstance';

export const getAllMedicalRecords = (params) => axiosInstance.get('/api/medicalrecords', { params });
export const getMedicalRecordById = (id) => axiosInstance.get(":path:/api/medicalrecords/ feedingSchedulesApi.js{id}");
export const getMedicalRecordsByAnimal = (animalId, params) => axiosInstance.get(":path:/api/medicalrecords/search/by-animal/ feedingSchedulesApi.js{animalId}", { params });
export const getMedicalRecordsByStaff = (staffId, params) => axiosInstance.get(":path:/api/medicalrecords/search/by-staff/ feedingSchedulesApi.js{staffId}", { params });
export const getMedicalRecordsByStatus = (status, params) => axiosInstance.get(":path:/api/medicalrecords/search/by-status/ feedingSchedulesApi.js{status}", { params });
export const getMedicalRecordsByDateRange = (params) => axiosInstance.get('/api/medicalrecords/search/by-date-range', { params });
export const getRecentMedicalRecords = (params) => axiosInstance.get('/api/medicalrecords/search/recent', { params });
export const createMedicalRecord = (data) => axiosInstance.post('/api/medicalrecords', data);
export const updateMedicalRecord = (id, data) => axiosInstance.put(":path:/api/medicalrecords/ feedingSchedulesApi.js{id}", data);
export const deleteMedicalRecord = (id) => axiosInstance.delete(":path:/api/medicalrecords/ feedingSchedulesApi.js{id}");

