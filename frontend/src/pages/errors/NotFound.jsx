// src/pages/errors/NotFound.jsx
import { Link } from 'react-router-dom';
import { Button } from '../../components/common/Button';

export const NotFound = () => {
  return (
    <div className=`min-h-screen bg-gray-50 flex items-center justify-center px-4`>
      <div className=`text-center`>
        <h1 className=`text-6xl font-bold text-gray-800 mb-4`>404</h1>
        <p className=`text-2xl text-gray-600 mb-8`>Page Not Found</p>
        <p className=`text-gray-500 mb-8`>
          The page you are looking for doesn't exist or has been moved.
        </p>
        <Link to=`/`>
          <Button>Go Back Home</Button>
        </Link>
      </div>
    </div>
  );
};
