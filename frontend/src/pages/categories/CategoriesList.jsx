// src/pages/categories/CategoriesList.jsx
import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { getAllCategories } from '../../api/categoriesApi';
import { useAuth } from '../../hooks/useAuth';
import { isZookeeperOrAbove } from '../../utils/roleCheck';
import { Card } from '../../components/common/Card';
import { Button } from '../../components/common/Button';
import { Spinner } from '../../components/common/Spinner';

export const CategoriesList = () => {
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  
  const { getUserRole } = useAuth();
  const userRole = getUserRole();

  useEffect(() => {
    fetchCategories();
  }, []);

  const fetchCategories = async () => {
    try {
      setLoading(true);
      const response = await getAllCategories();
      setCategories(response.data);
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
        <h1 className="text-3xl font-bold text-gray-800">Categories</h1>
        {isZookeeperOrAbove(userRole) && (
          <Link to="/categories/new">
            <Button>Add New Category</Button>
          </Link>
        )}
      </div>
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        {categories.map((category) => (
          <Card key={category.id} title={category.name}>
            <p className="text-gray-600 mb-4">{category.description}</p>
            <p className="text-sm text-gray-500">Animals: {category.animalCount || 0}</p>
          </Card>
        ))}
      </div>
    </div>
  );
};

