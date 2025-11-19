// src/api/imageUploadApi.js
import axiosInstance from './axiosInstance';

export const uploadImage = (formData) => {
  return axiosInstance.post('/api/imageupload/upload', formData, {
    headers: {
      'Content-Type': 'multipart/form-data',
    },
  });
};

export const uploadImageBase64 = (data) => {
  return axiosInstance.post('/api/imageupload/upload-base64', data);
};

export const uploadMultipleImages = (formData) => {
  return axiosInstance.post('/api/imageupload/upload-multiple', formData, {
    headers: {
      'Content-Type': 'multipart/form-data',
    },
  });
};

export const validateImage = (formData) => {
  return axiosInstance.post('/api/imageupload/validate', formData, {
    headers: {
      'Content-Type': 'multipart/form-data',
    },
  });
};
