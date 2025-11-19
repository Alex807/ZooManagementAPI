// src/pages/auth/Register.jsx
import { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';
import { AuthLayout } from '../../layouts/AuthLayout';
import { Input } from '../../components/common/Input';
import { Select } from '../../components/common/Select';
import { Button } from '../../components/common/Button';
import { Card } from '../../components/common/Card';

export const Register = () => {
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    birthDate: '',
    gender: '',
    phone: '',
    homeAddress: '',
    imageUrl: '',
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
    
    // First Name validation
    if (!formData.firstName.trim()) {
      newErrors.firstName = 'First name is required';
    } else if (formData.firstName.length > 50) {
      newErrors.firstName = 'First name cannot exceed 50 characters';
    }
    
    // Last Name validation
    if (!formData.lastName.trim()) {
      newErrors.lastName = 'Last name is required';
    } else if (formData.lastName.length > 50) {
      newErrors.lastName = 'Last name cannot exceed 50 characters';
    }
    
    // Birth Date validation
    if (!formData.birthDate) {
      newErrors.birthDate = 'Birth date is required';
    }
    
    // Phone validation (optional but must be valid if provided)
    if (formData.phone && !/^\d{10}$/.test(formData.phone)) {
      newErrors.phone = 'Phone number must contain exactly 10 digits';
    }
    
    // Home Address validation (optional)
    if (formData.homeAddress && formData.homeAddress.length > 255) {
      newErrors.homeAddress = 'Home address cannot exceed 255 characters';
    }
    
    // Image URL validation (optional)
    if (formData.imageUrl) {
      try {
        new URL(formData.imageUrl);
        if (formData.imageUrl.length > 2048) {
          newErrors.imageUrl = 'Image URL cannot exceed 2048 characters';
        }
      } catch {
        newErrors.imageUrl = 'Invalid URL format';
      }
    }
    
    // Username validation
    if (!formData.username.trim()) {
      newErrors.username = 'Username is required';
    } else if (formData.username.length < 3 || formData.username.length > 50) {
      newErrors.username = 'Username must be between 3 and 50 characters';
    }
    
    // Email validation
    if (!formData.email.trim()) {
      newErrors.email = 'Email is required';
    } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
      newErrors.email = 'Invalid email format';
    } else if (formData.email.length > 50) {
      newErrors.email = 'Email cannot exceed 50 characters';
    }
    
    // Password validation
    if (!formData.password) {
      newErrors.password = 'Password is required';
    } else if (formData.password.length < 8 || formData.password.length > 50) {
      newErrors.password = 'Password must be between 8 and 50 characters';
    } else if (!/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/.test(formData.password)) {
      newErrors.password = 'Password must contain at least one uppercase letter, one lowercase letter, one number and one special character (@ $ ! % * ? &)';
    }
    
    // Confirm Password validation
    if (!formData.confirmPassword) {
      newErrors.confirmPassword = 'Password confirmation is required';
    } else if (formData.password !== formData.confirmPassword) {
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
      firstName: formData.firstName,
      lastName: formData.lastName,
      birthDate: formData.birthDate,
      gender: formData.gender ? parseInt(formData.gender) : null,
      phone: formData.phone || '',
      homeAddress: formData.homeAddress || '',
      imageUrl: formData.imageUrl || '',
      username: formData.username,
      email: formData.email,
      password: formData.password,
      confirmPassword: formData.confirmPassword,
    });
    setLoading(false);

    if (result.success) {
      navigate('/dashboard');
    } else {
      // Handle both string errors and object errors from backend
      if (typeof result.error === 'string') {
        setErrors({ submit: result.error });
      } else if (typeof result.error === 'object') {
        // Backend returns validation errors in an object format
        const backendErrors = {};
        Object.keys(result.error).forEach(key => {
          // Convert backend field names to camelCase for form fields
          const fieldName = key.charAt(0).toLowerCase() + key.slice(1);
          backendErrors[fieldName] = Array.isArray(result.error[key]) 
            ? result.error[key].join(', ') 
            : result.error[key];
        });
        setErrors({ ...backendErrors, submit: 'Please fix the errors below' });
      } else {
        setErrors({ submit: 'Registration failed. Please try again.' });
      }
    }
  };

  const genderOptions = [
    { value: '0', label: 'Male' },
    { value: '1', label: 'Female' },
    { value: '2', label: 'Other' },
  ];

  return (
    <AuthLayout>
      <div className="w-full max-w-3xl mx-auto">
        <Card title="🦁 Register for Zoo Management System" className="shadow-xl">
          <p className="text-gray-600 mb-6 text-center">
            Join our zoo management team and help us care for amazing animals!
          </p>
          
          {errors.submit && (
            <div className="mb-6 p-4 bg-red-50 border-l-4 border-red-500 text-red-700 rounded-r-lg">
              <div className="flex items-start">
                <span className="text-xl mr-2">⚠️</span>
                <div>
                  <p className="font-semibold">Registration Error</p>
                  <p className="text-sm mt-1">{errors.submit}</p>
                </div>
              </div>
            </div>
          )}

          <form onSubmit={handleSubmit} className="space-y-6">
            {/* Personal Information Section */}
            <div className="bg-green-50 p-4 rounded-lg border border-green-200">
              <h3 className="text-lg font-semibold text-gray-800 mb-4 flex items-center">
                <span className="mr-2">👤</span> Personal Information
              </h3>
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <Input
                  label="First Name"
                  name="firstName"
                  value={formData.firstName}
                  onChange={handleChange}
                  error={errors.firstName}
                  required
                  placeholder="Enter your first name"
                  autoComplete="given-name"
                />
                <Input
                  label="Last Name"
                  name="lastName"
                  value={formData.lastName}
                  onChange={handleChange}
                  error={errors.lastName}
                  required
                  placeholder="Enter your last name"
                  autoComplete="family-name"
                />
                <Input
                  label="Birth Date"
                  type="date"
                  name="birthDate"
                  value={formData.birthDate}
                  onChange={handleChange}
                  error={errors.birthDate}
                  required
                  autoComplete="bday"
                />
                <Select
                  label="Gender"
                  name="gender"
                  value={formData.gender}
                  onChange={handleChange}
                  error={errors.gender}
                  options={genderOptions}
                  placeholder="Select gender (optional)"
                />
              </div>
            </div>

            {/* Contact Information Section */}
            <div className="bg-blue-50 p-4 rounded-lg border border-blue-200">
              <h3 className="text-lg font-semibold text-gray-800 mb-4 flex items-center">
                <span className="mr-2">📞</span> Contact Information
              </h3>
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <Input
                  label="Phone Number"
                  type="tel"
                  name="phone"
                  value={formData.phone}
                  onChange={handleChange}
                  error={errors.phone}
                  placeholder="1234567890 (10 digits)"
                  autoComplete="tel"
                />
                <Input
                  label="Home Address"
                  name="homeAddress"
                  value={formData.homeAddress}
                  onChange={handleChange}
                  error={errors.homeAddress}
                  placeholder="Enter your address (optional)"
                  autoComplete="street-address"
                />
              </div>
              <div className="mt-4">
                <Input
                  label="Profile Image URL"
                  type="url"
                  name="imageUrl"
                  value={formData.imageUrl}
                  onChange={handleChange}
                  error={errors.imageUrl}
                  placeholder="https://example.com/image.jpg (optional)"
                />
              </div>
            </div>

            {/* Account Information Section */}
            <div className="bg-purple-50 p-4 rounded-lg border border-purple-200">
              <h3 className="text-lg font-semibold text-gray-800 mb-4 flex items-center">
                <span className="mr-2">🔐</span> Account Credentials
              </h3>
              <div className="space-y-4">
                <Input
                  label="Username"
                  name="username"
                  value={formData.username}
                  onChange={handleChange}
                  error={errors.username}
                  required
                  placeholder="Choose a username (3-50 characters)"
                  autoComplete="username"
                />
                <Input
                  label="Email"
                  type="email"
                  name="email"
                  value={formData.email}
                  onChange={handleChange}
                  error={errors.email}
                  required
                  placeholder="your.email@example.com"
                  autoComplete="email"
                />
                <Input
                  label="Password"
                  type="password"
                  name="password"
                  value={formData.password}
                  onChange={handleChange}
                  error={errors.password}
                  required
                  placeholder="Minimum 8 characters"
                  autoComplete="new-password"
                />
                <div className="text-xs text-gray-600 bg-gray-50 p-3 rounded border border-gray-200">
                  <p className="font-semibold mb-1">Password Requirements:</p>
                  <ul className="list-disc list-inside space-y-1">
                    <li>At least 8 characters long</li>
                    <li>One uppercase and one lowercase letter</li>
                    <li>One number</li>
                    <li>One special character (@ $ ! % * ? &)</li>
                  </ul>
                </div>
                <Input
                  label="Confirm Password"
                  type="password"
                  name="confirmPassword"
                  value={formData.confirmPassword}
                  onChange={handleChange}
                  error={errors.confirmPassword}
                  required
                  placeholder="Re-enter your password"
                  autoComplete="new-password"
                />
              </div>
            </div>

            <Button 
              type="submit" 
              fullWidth 
              disabled={loading}
              className="py-3 text-lg font-semibold shadow-lg hover:shadow-xl transition-shadow"
            >
              {loading ? '⏳ Creating Account...' : '✨ Create Account'}
            </Button>
          </form>

          <div className="mt-6 pt-6 border-t border-gray-200">
            <p className="text-center text-sm text-gray-600">
              Already have an account?{' '}
              <Link to="/login" className="text-green-600 hover:text-green-700 font-semibold hover:underline">
                Login here →
              </Link>
            </p>
          </div>
        </Card>
      </div>
    </AuthLayout>
  );
};

