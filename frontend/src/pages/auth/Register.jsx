// src/pages/auth/Register.jsx
import { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';
import { AuthLayout } from '../../layouts/AuthLayout';
import { Input } from '../../components/common/Input';
import { Button } from '../../components/common/Button';
import { Card } from '../../components/common/Card';

export const Register = () => {
  const [formData, setFormData] = useState({
    username: '',
    email: '',
    password: '',
    confirmPassword: '',
  });
  const [errors, setErrors] = useState({});
  const [loading, setLoading] = useState(false);

  const { register, isAuthenticated } = useAuth();
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
    if (formData.username.length < 3) newErrors.username = 'Username must be at least 3 characters';
    if (!formData.email.trim()) newErrors.email = 'Email is required';
    if (!/\S+@\S+\.\S+/.test(formData.email)) newErrors.email = 'Email is invalid';
    if (!formData.password) newErrors.password = 'Password is required';
    if (formData.password.length < 6) newErrors.password = 'Password must be at least 6 characters';
    if (formData.password !== formData.confirmPassword) {
      newErrors.confirmPassword = 'Passwords do not match';
    }
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
    const result = await register({
      username: formData.username,
      email: formData.email,
      password: formData.password,
    });
    setLoading(false);

    if (result.success) {
      navigate('/dashboard');
    } else {
      setErrors({ submit: result.error });
    }
  };

  return (
    <AuthLayout>
      <Card title=`Register for Zoo Management`>
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
            label=`Email`
            type=`email`
            name=`email`
            value={formData.email}
            onChange={handleChange}
            error={errors.email}
            required
            autoComplete=`email`
          />
          <Input
            label=`Password`
            type=`password`
            name=`password`
            value={formData.password}
            onChange={handleChange}
            error={errors.password}
            required
            autoComplete=`new-password`
          />
          <Input
            label=`Confirm Password`
            type=`password`
            name=`confirmPassword`
            value={formData.confirmPassword}
            onChange={handleChange}
            error={errors.confirmPassword}
            required
            autoComplete=`new-password`
          />
          <Button type=`submit` fullWidth disabled={loading}>
            {loading ? 'Registering...' : 'Register'}
          </Button>
        </form>
        <p className=`mt-4 text-center text-sm text-gray-600`>
          Already have an account?{' '}
          <Link to=`/login` className=`text-green-600 hover:text-green-700 font-medium`>
            Login here
          </Link>
        </p>
      </Card>
    </AuthLayout>
  );
};
