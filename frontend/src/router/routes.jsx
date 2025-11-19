// src/router/routes.jsx
import { Routes, Route, Navigate } from 'react-router-dom';
import { ProtectedRoute } from './ProtectedRoute';
import { MainLayout } from '../layouts/MainLayout';

// Pages
import { Home } from '../pages/Home';
import { Login } from '../pages/auth/Login';
import { Register } from '../pages/auth/Register';
import { Dashboard } from '../pages/dashboard/Dashboard';
import { AnimalsList } from '../pages/animals/AnimalsList';
import { AnimalDetail } from '../pages/animals/AnimalDetail';
import { CategoriesList } from '../pages/categories/CategoriesList';
import { EnclosuresList } from '../pages/enclosures/EnclosuresList';
import { FeedingSchedulesList } from '../pages/feeding/FeedingSchedulesList';
import { MedicalRecordsList } from '../pages/medical/MedicalRecordsList';
import { StaffList } from '../pages/staff/StaffList';
import { AssignmentsList } from '../pages/assignments/AssignmentsList';
import { UsersList } from '../pages/users/UsersList';
import { Profile } from '../pages/profile/Profile';
import { NotFound } from '../pages/errors/NotFound';
import { Forbidden } from '../pages/errors/Forbidden';

export const AppRoutes = () => {
  return (
    <Routes>
      {/* Public Routes */}
      <Route path="/" element={<MainLayout><Home /></MainLayout>} />
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Register />} />
      
      {/* Public Animal Browsing */}
      <Route path="/animals" element={<MainLayout><AnimalsList /></MainLayout>} />
      <Route path="/animals/:id" element={<MainLayout><AnimalDetail /></MainLayout>} />
      <Route path="/categories" element={<MainLayout><CategoriesList /></MainLayout>} />
      <Route path="/enclosures" element={<MainLayout><EnclosuresList /></MainLayout>} />

      {/* Protected Routes - Authenticated */}
      <Route
        path="/dashboard"
        element={
          <ProtectedRoute>
            <MainLayout><Dashboard /></MainLayout>
          </ProtectedRoute>
        }
      />
      <Route
        path="/profile"
        element={
          <ProtectedRoute>
            <MainLayout><Profile /></MainLayout>
          </ProtectedRoute>
        }
      />
      <Route
        path="/feeding"
        element={
          <ProtectedRoute>
            <MainLayout><FeedingSchedulesList /></MainLayout>
          </ProtectedRoute>
        }
      />

      {/* Protected Routes - StaffOrAbove */}
      <Route
        path="/medical"
        element={
          <ProtectedRoute requiredRole="Veterinarian">
            <MainLayout><MedicalRecordsList /></MainLayout>
          </ProtectedRoute>
        }
      />
      <Route
        path="/staff"
        element={
          <ProtectedRoute requiredRole="Veterinarian">
            <MainLayout><StaffList /></MainLayout>
          </ProtectedRoute>
        }
      />
      <Route
        path="/assignments"
        element={
          <ProtectedRoute requiredRole="Veterinarian">
            <MainLayout><AssignmentsList /></MainLayout>
          </ProtectedRoute>
        }
      />

      {/* Protected Routes - AdminOnly */}
      <Route
        path="/users"
        element={
          <ProtectedRoute adminOnly>
            <MainLayout><UsersList /></MainLayout>
          </ProtectedRoute>
        }
      />

      {/* Error Routes */}
      <Route path="/403" element={<Forbidden />} />
      <Route path="/404" element={<NotFound />} />
      <Route path="*" element={<NotFound />} />
    </Routes>
  );
};

