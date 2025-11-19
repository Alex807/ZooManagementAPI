// src/pages/auth/Login.jsx
import { useState, useEffect } from 'react';
import { Link, useNavigate, useSearchParams } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';
import { AuthLayout } from '../../layouts/AuthLayout';
import { Input } from '../../components/common/Input';
import { Button } from '../../components/common/Button';
import { Card } from '../../components/common/Card';

export const Login = () => {
  const [formData, setFormData] = useState({
    username: '',
    password: '',
  });
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);
  const [searchParams] = useSearchParams();

  const { login, isAuthenticated } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    if (isAuthenticated()) {
      navigate('/dashboard');
    }
  }, [isAuthenticated, navigate]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
    if (errors[name]) {
      setErrors((prev) => ({ ...prev, [name]: null }));
    }
  };

  const validate = () => {
    const newErrors = {};
    if (!formData.username.trim()) newErrors.username = 'Username is required';
    if (!formData.password) newErrors.password = 'Password is required';
    return newErrors;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const newErrors = validate();
    if (Object.keys(newErrors).length > 0) {
      setErrors(newErrors);
      return;
    }

    setLoading(true);
    const result = await login(formData);
    setLoading(false);

    if (result.success) {
      navigate('/dashboard');
    } else {
      setErrors({ submit: result.error });
    }
  };

  const sessionExpired = searchParams.get('sessionExpired');

  return (
    <AuthLayout>
      <Card title=`Login to Zoo Management`>
        {sessionExpired && (
          <div className=`mb-4 p-3 bg-amber-100 border border-amber-400 text-amber-700 rounded`>
            Your session has expired. Please login again.
          </div>
        )}
        {errors.submit && (
          <div className=`mb-4 p-3 bg-red-100 border border-red-400 text-red-700 rounded`>
            {errors.submit}
          </div>
        )}
        <form onSubmit={handleSubmit} className=`space-y-4`>
          <Input
            label=`Username`
            name=`username`
            value={formData.username}
            onChange={handleChange}
            error={errors.username}
            required
            autoComplete=`username`
          />
          <Input
            label=`Password`
            type=`password`
            name=`password`
            value={formData.password}
            onChange={handleChange}
            error={errors.password}
            required
            autoComplete=`current-password`
          />
          <Button type=`submit` fullWidth disabled={loading}>
            {loading ? 'Logging in...' : 'Login'}
          </Button>
        </form>
        <p className=`mt-4 text-center text-sm text-gray-600`>
          Don't have an account?{' '}
          <Link to=`/register` className=`text-green-600 hover:text-green-700 font-medium`>
            Register here
          </Link>
        </p>
      </Card>
    </AuthLayout>
  );
};
