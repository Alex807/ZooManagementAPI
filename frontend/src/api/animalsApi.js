// src/api/animalsApi.js
import axiosInstance from './axiosInstance';

export const getAllAnimals = (params) => axiosInstance.get('/api/animals', { params });
export const getAnimalById = (id) => axiosInstance.get(":path:/api/animals/ usersApi.js{id}");
export const getAnimalsByCategory = (categoryId, params) => axiosInstance.get(":path:/api/animals/search/by-category/ usersApi.js{categoryId}", { params });
export const getAnimalsByEnclosure = (enclosureId, params) => axiosInstance.get(":path:/api/animals/search/by-enclosure/ usersApi.js{enclosureId}", { params });
export const getAnimalsBySpecie = (params) => axiosInstance.get('/api/animals/search/by-specie', { params });
export const getAnimalsByGender = (gender, params) => axiosInstance.get(":path:/api/animals/search/by-gender/ usersApi.js{gender}", { params });
export const getAnimalsByAgeRange = (params) => axiosInstance.get('/api/animals/search/by-age-range', { params });
export const getAnimalsByArrivalDate = (params) => axiosInstance.get('/api/animals/search/by-arrival-date', { params });
export const createAnimal = (data) => axiosInstance.post('/api/animals', data);
export const updateAnimal = (id, data) => axiosInstance.put(":path:/api/animals/ usersApi.js{id}", data);
export const deleteAnimal = (id) => axiosInstance.delete(":path:/api/animals/ usersApi.js{id}");

