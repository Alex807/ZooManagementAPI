// src/pages/Home.jsx
import { Link } from 'react-router-dom';
import { Button } from '../components/common/Button';
import { Card } from '../components/common/Card';
import { useAuth } from '../hooks/useAuth';
import { isZookeeperOrAbove } from '../utils/roleCheck';

export const Home = () => {
  const { getUserRole, isAuthenticated } = useAuth();
  const userRole = getUserRole();
  const canManage = isAuthenticated() && isZookeeperOrAbove(userRole);

  return (
    <div className="min-h-screen bg-gradient-to-b from-green-50 to-white">
      {/* Hero Section */}
      <div className="text-center py-16 px-4">
        <h1 className="text-5xl md:text-6xl font-bold text-gray-800 mb-6 leading-tight">
          Welcome to Zoo Management System
        </h1>
        <p className="text-xl md:text-2xl text-gray-600 mb-10 max-w-3xl mx-auto">
          Explore our amazing collection of animals, learn about their habitats, and discover the wonders of wildlife conservation.
        </p>
        <div className="flex flex-col sm:flex-row justify-center gap-4 sm:space-x-4">
          <Link to="/animals">
            <Button size="lg" className="w-full sm:w-auto shadow-lg hover:shadow-xl transition-shadow">
              🦁 Browse Animals
            </Button>
          </Link>
          <Link to="/login">
            <Button size="lg" variant="outline" className="w-full sm:w-auto shadow-lg hover:shadow-xl transition-shadow">
              👨‍💼 Staff Login
            </Button>
          </Link>
        </div>
      </div>

      {/* Feature Cards Section */}
      <div className="max-w-7xl mx-auto px-4 pb-12">
        <div className="grid grid-cols-1 md:grid-cols-3 gap-8 mb-12">
          <Card title="🦒 Animals" className="hover:shadow-xl transition-shadow duration-300 border-t-4 border-green-500">
            <p className="text-gray-600 mb-6 text-base leading-relaxed">
              Discover our diverse collection of animals from around the world, each with unique characteristics and stories.
            </p>
            <div className="flex flex-col gap-3">
              <Link to="/animals" className="inline-flex items-center text-green-600 hover:text-green-700 font-semibold hover:underline">
                View All Animals →
              </Link>
              {canManage && (
                <Link to="/animals/new">
                  <Button size="sm" fullWidth className="shadow-md hover:shadow-lg transition-shadow">
                    ➕ Create New Animal
                  </Button>
                </Link>
              )}
            </div>
          </Card>

          <Card title="🦜 Categories" className="hover:shadow-xl transition-shadow duration-300 border-t-4 border-blue-500">
            <p className="text-gray-600 mb-6 text-base leading-relaxed">
              Browse animals by categories such as mammals, birds, reptiles, and more to learn about different species.
            </p>
            <div className="flex flex-col gap-3">
              <Link to="/categories" className="inline-flex items-center text-green-600 hover:text-green-700 font-semibold hover:underline">
                View Categories →
              </Link>
              {canManage && (
                <Link to="/categories/new">
                  <Button size="sm" fullWidth className="shadow-md hover:shadow-lg transition-shadow">
                    ➕ Create New Category
                  </Button>
                </Link>
              )}
            </div>
          </Card>

          <Card title="🏞️ Enclosures" className="hover:shadow-xl transition-shadow duration-300 border-t-4 border-amber-500">
            <p className="text-gray-600 mb-6 text-base leading-relaxed">
              Learn about the different habitats and enclosures designed to provide the best environment for our animals.
            </p>
            <div className="flex flex-col gap-3">
              <Link to="/enclosures" className="inline-flex items-center text-green-600 hover:text-green-700 font-semibold hover:underline">
                View Enclosures →
              </Link>
              {canManage && (
                <Link to="/enclosures/new">
                  <Button size="sm" fullWidth className="shadow-md hover:shadow-lg transition-shadow">
                    ➕ Create New Enclosure
                  </Button>
                </Link>
              )}
            </div>
          </Card>
        </div>

        {/* About Section */}
        <Card title="🌿 About Our Zoo" className="shadow-lg border-l-4 border-green-600">
          <p className="text-gray-700 text-lg leading-relaxed">
            Our zoo is dedicated to conservation, education, and providing the best care for our animals.
            We house over <span className="font-semibold text-green-700">500 animals</span> from <span className="font-semibold text-green-700">150 different species</span>, 
            ranging from majestic lions to colorful tropical birds. Our mission is to inspire people to care about wildlife and the natural world,
            fostering a deeper connection between humans and nature.
          </p>
        </Card>
      </div>
    </div>
  );
};

