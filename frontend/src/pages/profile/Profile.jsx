// src/pages/profile/Profile.jsx
import { useAuth } from '../../hooks/useAuth';
import { Card } from '../../components/common/Card';

export const Profile = () => {
  const { user } = useAuth();

  return (
    <div className="space-y-6">
      <h1 className="text-3xl font-bold text-gray-800">My Profile</h1>
      <Card title="Profile Information">
        <div className="space-y-2">
          <p><strong>Username:</strong> {user?.username}</p>
          <p><strong>Email:</strong> {user?.email}</p>
          <p><strong>Role:</strong> {user?.role}</p>
        </div>
      </Card>
      <Card>
        <p className="text-gray-600">Profile editing feature coming soon...</p>
      </Card>
    </div>
  );
};

