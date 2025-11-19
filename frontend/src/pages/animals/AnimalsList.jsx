// src/pages/animals/AnimalsList.jsx
import { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { getAllAnimals } from '../../api/animalsApi';
import { useAuth } from '../../hooks/useAuth';
import { isZookeeperOrAbove } from '../../utils/roleCheck';
import { Card } from '../../components/common/Card';
import { Button } from '../../components/common/Button';
import { Spinner } from '../../components/common/Spinner';
import { Badge } from '../../components/common/Badge';
import { Input } from '../../components/common/Input';

export const AnimalsList = () => {
  const [animals, setAnimals] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  
  const { getUserRole } = useAuth();
  const navigate = useNavigate();
  const userRole = getUserRole();

  useEffect(() => {
    fetchAnimals();
  }, []);

  const fetchAnimals = async () => {
    try {
      setLoading(true);
      const response = await getAllAnimals();
      setAnimals(response.data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const filteredAnimals = animals.filter(animal =>
    animal.name?.toLowerCase().includes(searchTerm.toLowerCase()) ||
    animal.specie?.toLowerCase().includes(searchTerm.toLowerCase())
  );

  if (loading) {
    return (
      <div className="flex justify-center items-center min-h-[400px]">
        <Spinner size="lg" />
      </div>
    );
  }

  if (error) {
    return (
      <Card>
        <p className="text-red-600">Error: {error}</p>
        <Button onClick={fetchAnimals} className="mt-4">Retry</Button>
      </Card>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-3xl font-bold text-gray-800">Animals</h1>
        {isZookeeperOrAbove(userRole) && (
          <Link to="/animals/new">
            <Button>Add New Animal</Button>
          </Link>
        )}
      </div>

      <Card>
        <Input
          placeholder="Search animals by name or species..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
      </Card>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {filteredAnimals.length === 0 ? (
          <Card>
            <p className="text-gray-600">No animals found.</p>
          </Card>
        ) : (
          filteredAnimals.map((animal) => (
            <Card key={animal.id}>
              {animal.imageUrl && (
                <img
                  src={animal.imageUrl}
                  alt={animal.name}
                  className="w-full h-48 object-cover rounded-t-lg -mt-6 -mx-6 mb-4"
                />
              )}
              <h3 className="text-xl font-semibold text-gray-800 mb-2">{animal.name}</h3>
              <p className="text-gray-600 mb-2">{animal.specie}</p>
              <div className="flex flex-wrap gap-2 mb-4">
                <Badge variant="primary">{animal.gender}</Badge>
                <Badge variant={animal.healthStatus === 'Healthy' ? 'success' : 'warning'}>
                  {animal.healthStatus}
                </Badge>
              </div>
              <Button
                size="sm"
                variant="outline"
                fullWidth
                onClick={() => navigate('/animals/' + animal.id)}
              >
                View Details
              </Button>
            </Card>
          ))
        )}
      </div>
    </div>
  );
};