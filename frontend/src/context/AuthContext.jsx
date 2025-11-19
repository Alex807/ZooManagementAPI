// src/context/AuthContext.jsx
import { createContext, useState, useEffect } from 'react';
import { login as loginApi, register as registerApi } from '../api/authApi';
import { getCurrentUser } from '../api/usersApi';

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [token, setToken] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Initialize auth state from localStorage
  useEffect(() => {
    const initAuth = async () => {
      const storedToken = localStorage.getItem('token');
      const storedUser = localStorage.getItem('user');

      if (storedToken && storedUser) {
        try {
          setToken(storedToken);
          setUser(JSON.parse(storedUser));
          
          // Optionally verify token by fetching current user
          const response = await getCurrentUser();
          setUser(response.data);
          localStorage.setItem('user', JSON.stringify(response.data));
        } catch (err) {
          console.error('Token verification failed:', err);
          logout();
        }
      }
      setLoading(false);
    };

    initAuth();
  }, []);

  const login = async (credentials) => {
    try {
      setError(null);
      const response = await loginApi(credentials);
      const { token: newToken, username, email, currentRoleName } = response.data;

      const userData = {
        username,
        email,
        role: currentRoleName,
      };

      setToken(newToken);
      setUser(userData);

      localStorage.setItem('token', newToken);
      localStorage.setItem('user', JSON.stringify(userData));

      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Login failed';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    }
  };

  const register = async (userData) => {
    try {
      setError(null);
      const response = await registerApi(userData);
      const { token: newToken, username, email, currentRoleName } = response.data;

      const user = {
        username,
        email,
        role: currentRoleName,
      };

      setToken(newToken);
      setUser(user);

      localStorage.setItem('token', newToken);
      localStorage.setItem('user', JSON.stringify(user));

      return { success: true };
    } catch (err) {
      const errorMessage = err.message || 'Registration failed';
      setError(errorMessage);
      return { success: false, error: errorMessage };
    }
  };

  const logout = () => {
    setUser(null);
    setToken(null);
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  };

  const updateUser = (updatedUserData) => {
    const updatedUser = { ...user, ...updatedUserData };
    setUser(updatedUser);
    localStorage.setItem('user', JSON.stringify(updatedUser));
  };

  const isAuthenticated = () => {
    return !!token && !!user;
  };

  const getUserRole = () => {
    return user?.role || 'Visitor';
  };

  return (
    <AuthContext.Provider
      value={{
        user,
        token,
        loading,
        error,
        login,
        register,
        logout,
        updateUser,
        isAuthenticated,
        getUserRole,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};
