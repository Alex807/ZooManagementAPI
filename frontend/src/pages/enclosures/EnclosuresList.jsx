// src/pages/enclosures/EnclosuresList.jsx
import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { getAllEnclosures } from '../../api/enclosuresApi';
import { useAuth } from '../../hooks/useAuth';
import { isZookeeperOrAbove } from '../../utils/roleCheck';
import { Card } from '../../components/common/Card';
import { Button } from '../../components/common/Button';
import { Spinner } from '../../components/common/Spinner';

export const EnclosuresList = () => {
  const [enclosures, setEnclosures] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  
  const { getUserRole } = useAuth();
  const userRole = getUserRole();

  useEffect(() => {
    fetchEnclosures();
  }, []);

  const fetchEnclosures = async () => {
    try {
      setLoading(true);
      const response = await getAllEnclosures();
      setEnclosures(response.data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <div className="flex justify-center py-8"><Spinner size="lg" /></div>;
  if (error) return <Card><p className="text-red-600">Error: {error}</p></Card>;

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-3xl font-bold text-gray-800">Enclosures</h1>
        {isZookeeperOrAbove(userRole) && (
          <Link to="/enclosures/new">
            <Button>Add New Enclosure</Button>
          </Link>
        )}
      </div>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        {enclosures.map((enclosure) => (
          <Card key={enclosure.id} title={enclosure.name}>
            <p className="text-gray-600 mb-2">{enclosure.type}</p>
            <p className="text-sm text-gray-500">Location: {enclosure.location}</p>
            <p className="text-sm text-gray-500">Capacity: {enclosure.currentOccupancy || 0} / {enclosure.capacity}</p>
          </Card>
        ))}
      </div>
    </div>
  );
};

