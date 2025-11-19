// src/api/categoriesApi.js
import axiosInstance from './axiosInstance';

export const getAllCategories = (params) => axiosInstance.get('/api/categories', { params });
export const getCategoryById = (id) => axiosInstance.get(":path:/api/categories/ animalsApi.js{id}");
export const getCategoriesByName = (params) => axiosInstance.get('/api/categories/search/by-name', { params });
export const getCategoriesByAnimalCount = (params) => axiosInstance.get('/api/categories/search/by-animal-count', { params });
export const getEmptyCategories = () => axiosInstance.get('/api/categories/search/empty');
export const createCategory = (data) => axiosInstance.post('/api/categories', data);
export const updateCategory = (id, data) => axiosInstance.put(":path:/api/categories/ animalsApi.js{id}", data);
export const deleteCategory = (id) => axiosInstance.delete(":path:/api/categories/ animalsApi.js{id}");

