// src/pages/dashboard/Dashboard.jsx
import { Link } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';
import { Card } from '../../components/common/Card';
import { Button } from '../../components/common/Button';
import { isAdmin, isStaffOrAbove, isZookeeperOrAbove, isVeterinarianOrAbove } from '../../utils/roleCheck';

export const Dashboard = () => {
  const { user, getUserRole } = useAuth();
  const userRole = getUserRole();

  const quickActions = [];

  // Role-specific quick actions
  if (isZookeeperOrAbove(userRole)) {
    quickActions.push(
      { label: 'Add Animal', link: '/animals/new', color: 'bg-green-500' },
      { label: 'Add Category', link: '/categories/new', color: 'bg-blue-500' },
      { label: 'Add Enclosure', link: '/enclosures/new', color: 'bg-amber-500' }
    );
  }

  if (isStaffOrAbove(userRole)) {
    quickActions.push(
      { label: 'View Feeding Schedule', link: '/feeding', color: 'bg-purple-500' },
      { label: 'View Assignments', link: '/assignments', color: 'bg-indigo-500' }
    );
  }

  if (isVeterinarianOrAbove(userRole)) {
    quickActions.push(
      { label: 'Add Medical Record', link: '/medical/new', color: 'bg-red-500' }
    );
  }

  if (isAdmin(userRole)) {
    quickActions.push(
      { label: 'Manage Users', link: '/users', color: 'bg-gray-700' },
      { label: 'Manage Staff', link: '/staff', color: 'bg-teal-500' }
    );
  }

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-3xl font-bold text-gray-800">Welcome, {user?.username}!</h1>
        <p className="text-gray-600 mt-2">Role: {userRole}</p>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        <Card title="Animals">
          <p className="text-3xl font-bold text-green-600">-</p>
          <p className="text-sm text-gray-600 mt-2">Total animals in the zoo</p>
          <Link to="/animals" className="text-green-600 hover:underline mt-2 inline-block">
            View all 
          </Link>
        </Card>

        <Card title="Categories">
          <p className="text-3xl font-bold text-blue-600">-</p>
          <p className="text-sm text-gray-600 mt-2">Animal categories</p>
          <Link to="/categories" className="text-blue-600 hover:underline mt-2 inline-block">
            View all 
          </Link>
        </Card>

        <Card title="Enclosures">
          <p className="text-3xl font-bold text-amber-600">-</p>
          <p className="text-sm text-gray-600 mt-2">Total enclosures</p>
          <Link to="/enclosures" className="text-amber-600 hover:underline mt-2 inline-block">
            View all 
          </Link>
        </Card>

        {isStaffOrAbove(userRole) && (
          <Card title="Staff">
            <p className="text-3xl font-bold text-purple-600">-</p>
            <p className="text-sm text-gray-600 mt-2">Total staff members</p>
            <Link to="/staff" className="text-purple-600 hover:underline mt-2 inline-block">
              View all 
            </Link>
          </Card>
        )}
      </div>

      {quickActions.length > 0 && (
        <Card title="Quick Actions">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            {quickActions.map((action, index) => (
              <Link key={index} to={action.link}>
                <Button
                  className={"w-full  Register.jsx{action.color} hover:opacity-90"}
                  variant="primary"
                >
                  {action.label}
                </Button>
              </Link>
            ))}
          </div>
        </Card>
      )}

      <Card title="Recent Activity">
        <p className="text-gray-600">No recent activity to display.</p>
      </Card>
    </div>
  );
};

