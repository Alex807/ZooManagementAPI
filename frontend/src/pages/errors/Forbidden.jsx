// src/pages/errors/Forbidden.jsx
import { Link } from 'react-router-dom';
import { Button } from '../../components/common/Button';

export const Forbidden = () => {
  return (
    <div className="min-h-screen bg-gray-50 flex items-center justify-center px-4">
      <div className="text-center">
        <h1 className="text-6xl font-bold text-red-600 mb-4">403</h1>
        <p className="text-2xl text-gray-800 mb-4">Access Denied</p>
        <p className="text-gray-600 mb-8">
          You don't have permission to access this resource.
        </p>
        <Link to="/">
          <Button>Go Back Home</Button>
        </Link>
      </div>
    </div>
  );
};

