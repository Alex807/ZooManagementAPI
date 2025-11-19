// src/api/enclosuresApi.js
import axiosInstance from './axiosInstance';

export const getAllEnclosures = (params) => axiosInstance.get('/api/enclosures', { params });
export const getEnclosureById = (id) => axiosInstance.get(":path:/api/enclosures/ categoriesApi.js{id}");
export const getEnclosuresByName = (params) => axiosInstance.get('/api/enclosures/search/by-name', { params });
export const getEnclosuresByType = (params) => axiosInstance.get('/api/enclosures/search/by-type', { params });
export const getEnclosuresByLocation = (params) => axiosInstance.get('/api/enclosures/search/by-location', { params });
export const getEnclosuresByCapacity = (params) => axiosInstance.get('/api/enclosures/search/by-capacity', { params });
export const getEnclosuresAtCapacity = () => axiosInstance.get('/api/enclosures/search/at-capacity');
export const getAvailableEnclosures = () => axiosInstance.get('/api/enclosures/search/available');
export const getEmptyEnclosures = () => axiosInstance.get('/api/enclosures/search/empty');
export const createEnclosure = (data) => axiosInstance.post('/api/enclosures', data);
export const updateEnclosure = (id, data) => axiosInstance.put(":path:/api/enclosures/ categoriesApi.js{id}", data);
export const deleteEnclosure = (id) => axiosInstance.delete(":path:/api/enclosures/ categoriesApi.js{id}");

