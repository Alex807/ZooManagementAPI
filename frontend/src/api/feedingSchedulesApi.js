// src/api/feedingSchedulesApi.js
import axiosInstance from './axiosInstance';

export const getAllFeedingSchedules = (params) => axiosInstance.get('/api/feedingschedules', { params });
export const getFeedingScheduleById = (id) => axiosInstance.get(`:path:/api/feedingschedules/ enclosuresApi.js{id}`);
export const getFeedingSchedulesByAnimal = (animalId, params) => axiosInstance.get(`:path:/api/feedingschedules/search/by-animal/ enclosuresApi.js{animalId}`, { params });
export const getFeedingSchedulesByStaff = (staffId, params) => axiosInstance.get(`:path:/api/feedingschedules/search/by-staff/ enclosuresApi.js{staffId}`, { params });
export const getFeedingSchedulesByStatus = (status, params) => axiosInstance.get(`:path:/api/feedingschedules/search/by-status/ enclosuresApi.js{status}`, { params });
export const getFeedingSchedulesByFoodType = (params) => axiosInstance.get('/api/feedingschedules/search/by-food-type', { params });
export const getFeedingSchedulesByDateRange = (params) => axiosInstance.get('/api/feedingschedules/search/by-date-range', { params });
export const getTodayFeedingSchedules = () => axiosInstance.get('/api/feedingschedules/search/today');
export const getUpcomingFeedingSchedules = (params) => axiosInstance.get('/api/feedingschedules/search/upcoming', { params });
export const getOverdueFeedingSchedules = () => axiosInstance.get('/api/feedingschedules/search/overdue');
export const createFeedingSchedule = (data) => axiosInstance.post('/api/feedingschedules', data);
export const updateFeedingSchedule = (id, data) => axiosInstance.put(`:path:/api/feedingschedules/ enclosuresApi.js{id}`, data);
export const deleteFeedingSchedule = (id) => axiosInstance.delete(`:path:/api/feedingschedules/ enclosuresApi.js{id}`);
