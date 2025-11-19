// src/pages/animals/AnimalDetail.jsx
import { useState, useEffect } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { getAnimalById, deleteAnimal } from '../../api/animalsApi';
import { useAuth } from '../../hooks/useAuth';
import { isZookeeperOrAbove, isAdmin } from '../../utils/roleCheck';
import { Card } from '../../components/common/Card';
import { Button } from '../../components/common/Button';
import { Spinner } from '../../components/common/Spinner';
import { Badge } from '../../components/common/Badge';
import { Modal } from '../../components/common/Modal';

export const AnimalDetail = () => {
  const { id } = useParams();
  const [animal, setAnimal] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [deleting, setDeleting] = useState(false);
  
  const { getUserRole } = useAuth();
  const navigate = useNavigate();
  const userRole = getUserRole();

  useEffect(() => {
    fetchAnimal();
  }, [id]);

  const fetchAnimal = async () => {
    try {
      setLoading(true);
      const response = await getAnimalById(id);
      setAnimal(response.data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async () => {
    try {
      setDeleting(true);
      await deleteAnimal(id);
      navigate('/animals');
    } catch (err) {
      setError(err.message);
    } finally {
      setDeleting(false);
      setShowDeleteModal(false);
    }
  };

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
        <Button onClick={fetchAnimal} className="mt-4">Retry</Button>
      </Card>
    );
  }

  if (!animal) {
    return (
      <Card>
        <p className="text-gray-600">Animal not found.</p>
      </Card>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-3xl font-bold text-gray-800">{animal.name}</h1>
        <div className="flex space-x-2">
          <Link to="/animals">
            <Button variant="outline">Back to List</Button>
          </Link>
          {isZookeeperOrAbove(userRole) && (
            <Link to={`/animals/${id}/edit`}>
              <Button>Edit</Button>
            </Link>
          )}
          {isAdmin(userRole) && (
            <Button variant="danger" onClick={() => setShowDeleteModal(true)}>
              Delete
            </Button>
          )}
        </div>
      </div>

      <Card>
        {animal.imageUrl && (
          <img
            src={animal.imageUrl}
            alt={animal.name}
            className="w-full h-96 object-cover rounded-lg mb-6"
          />
        )}
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div>
            <h3 className="text-lg font-semibold text-gray-700 mb-2">Basic Information</h3>
            <div className="space-y-2">
              <p><strong>Name:</strong> {animal.name}</p>
              <p><strong>Species:</strong> {animal.specie}</p>
              <p><strong>Gender:</strong> {animal.gender}</p>
              <p><strong>Age:</strong> {animal.age} years</p>
              <p>
                <strong>Health Status:</strong>{' '}
                <Badge variant={animal.healthStatus === 'Healthy' ? 'success' : 'warning'}>
                  {animal.healthStatus}
                </Badge>
              </p>
            </div>
          </div>
          <div>
            <h3 className="text-lg font-semibold text-gray-700 mb-2">Additional Details</h3>
            <div className="space-y-2">
              <p><strong>Category:</strong> {animal.categoryName || 'N/A'}</p>
              <p><strong>Enclosure:</strong> {animal.enclosureName || 'Not assigned'}</p>
              <p><strong>Arrival Date:</strong> {animal.arrivalDate ? new Date(animal.arrivalDate).toLocaleDateString() : 'N/A'}</p>
            </div>
          </div>
        </div>
        {animal.description && (
          <div className="mt-6">
            <h3 className="text-lg font-semibold text-gray-700 mb-2">Description</h3>
            <p className="text-gray-600">{animal.description}</p>
          </div>
        )}
      </Card>

      <Modal
        isOpen={showDeleteModal}
        onClose={() => setShowDeleteModal(false)}
        title="Confirm Deletion"
        footer={
          <div className="flex justify-end space-x-2">
            <Button variant="outline" onClick={() => setShowDeleteModal(false)}>
              Cancel
            </Button>
            <Button variant="danger" onClick={handleDelete} disabled={deleting}>
              {deleting ? 'Deleting...' : 'Delete'}
            </Button>
          </div>
        }
      >
        <p>Are you sure you want to delete {animal.name}? This action cannot be undone.</p>
      </Modal>
    </div>
  );
};
