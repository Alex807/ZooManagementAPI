// src/components/common/Button.jsx
export const Button = ({
  children,
  onClick,
  type = 'button',
  variant = 'primary',
  size = 'md',
  disabled = false,
  fullWidth = false,
  className = '',
  ...props
}) => {
  const baseStyles = 'font-medium rounded-lg transition-colors duration-200 focus:outline-none focus:ring-2 focus:ring-offset-2';

  const variantStyles = {
    primary: 'bg-green-600 text-white hover:bg-green-700 focus:ring-green-500 disabled:bg-green-300',
    secondary: 'bg-gray-600 text-white hover:bg-gray-700 focus:ring-gray-500 disabled:bg-gray-300',
    danger: 'bg-red-600 text-white hover:bg-red-700 focus:ring-red-500 disabled:bg-red-300',
    outline: 'border-2 border-green-600 text-green-600 hover:bg-green-50 focus:ring-green-500 disabled:border-green-300 disabled:text-green-300',
    ghost: 'text-green-600 hover:bg-green-50 focus:ring-green-500 disabled:text-green-300',
  };

  const sizeStyles = {
    sm: 'px-3 py-1.5 text-sm',
    md: 'px-4 py-2 text-base',
    lg: 'px-6 py-3 text-lg',
  };

  const widthStyles = fullWidth ? 'w-full' : '';

  return (
    <button
      type={type}
      onClick={onClick}
      disabled={disabled}
      className={" ProtectedRoute.jsx{baseStyles}  ProtectedRoute.jsx{variantStyles[variant]}  ProtectedRoute.jsx{sizeStyles[size]}  ProtectedRoute.jsx{widthStyles}  ProtectedRoute.jsx{className}"}
      {...props}
    >
      {children}
    </button>
  );
};

