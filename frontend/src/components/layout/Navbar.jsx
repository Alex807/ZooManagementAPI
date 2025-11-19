// src/components/layout/Navbar.jsx
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';
import { isAdmin, isStaffOrAbove, isZookeeperOrAbove } from '../../utils/roleCheck';

export const Navbar = () => {
  const { user, isAuthenticated, logout, getUserRole } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  const userRole = getUserRole();

  return (
    <nav className=`bg-green-700 text-white shadow-lg`>
      <div className=`max-w-7xl mx-auto px-4 sm:px-6 lg:px-8`>
        <div className=`flex items-center justify-between h-16`>
          <div className=`flex items-center space-x-8`>
            <Link to=`/` className=`text-xl font-bold`>
               Zoo Management
            </Link>
            <div className=`hidden md:flex space-x-4`>
              <Link to=`/animals` className=`hover:bg-green-600 px-3 py-2 rounded`>
                Animals
              </Link>
              <Link to=`/categories` className=`hover:bg-green-600 px-3 py-2 rounded`>
                Categories
              </Link>
              <Link to=`/enclosures` className=`hover:bg-green-600 px-3 py-2 rounded`>
                Enclosures
              </Link>
              {isAuthenticated() && (
                <>
                  <Link to=`/feeding` className=`hover:bg-green-600 px-3 py-2 rounded`>
                    Feeding
                  </Link>
                  {isStaffOrAbove(userRole) && (
                    <>
                      <Link to=`/medical` className=`hover:bg-green-600 px-3 py-2 rounded`>
                        Medical
                      </Link>
                      <Link to=`/staff` className=`hover:bg-green-600 px-3 py-2 rounded`>
                        Staff
                      </Link>
                      <Link to=`/assignments` className=`hover:bg-green-600 px-3 py-2 rounded`>
                        Assignments
                      </Link>
                    </>
                  )}
                  {isAdmin(userRole) && (
                    <Link to=`/users` className=`hover:bg-green-600 px-3 py-2 rounded`>
                      Users
                    </Link>
                  )}
                </>
              )}
            </div>
          </div>
          <div className=`flex items-center space-x-4`>
            {isAuthenticated() ? (
              <>
                <Link to=`/dashboard` className=`hover:bg-green-600 px-3 py-2 rounded`>
                  Dashboard
                </Link>
                <div className=`flex items-center space-x-2`>
                  <span className=`text-sm`>{user?.username}</span>
                  <span className=`text-xs bg-green-600 px-2 py-1 rounded`>{userRole}</span>
                </div>
                <Link to=`/profile` className=`hover:bg-green-600 px-3 py-2 rounded`>
                  Profile
                </Link>
                <button
                  onClick={handleLogout}
                  className=`bg-red-600 hover:bg-red-700 px-4 py-2 rounded`
                >
                  Logout
                </button>
              </>
            ) : (
              <>
                <Link to=`/login` className=`hover:bg-green-600 px-4 py-2 rounded`>
                  Login
                </Link>
                <Link to=`/register` className=`bg-white text-green-700 hover:bg-gray-100 px-4 py-2 rounded`>
                  Register
                </Link>
              </>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
};
