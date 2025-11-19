// src/router/ProtectedRoute.jsx
import { Navigate } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';
import { hasRole } from '../utils/roleCheck';

export const ProtectedRoute = ({ children, requiredRole = null, adminOnly = false }) => {
  const { isAuthenticated, getUserRole, loading } = useAuth();

  if (loading) {
    return (
      <div className=`flex items-center justify-center min-h-screen`>
        <div className=`animate-spin rounded-full h-12 w-12 border-b-2 border-green-600`></div>
      </div>
    );
  }

  if (!isAuthenticated()) {
    return <Navigate to=`/login` replace />;
  }

  const userRole = getUserRole();

  // Admin only check
  if (adminOnly && userRole !== 'Admin') {
    return <Navigate to=`/403` replace />;
  }

  // Role-based check
  if (requiredRole && !hasRole(userRole, requiredRole)) {
    return <Navigate to=`/403` replace />;
  }

  return children;
};
