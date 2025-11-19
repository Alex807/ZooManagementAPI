// src/components/common/Spinner.jsx
export const Spinner = ({ size = 'md', className = '' }) => {
  const sizeStyles = {
    sm: 'h-4 w-4',
    md: 'h-8 w-8',
    lg: 'h-12 w-12',
  };

  return (
    <div className={`animate-spin rounded-full border-b-2 border-green-600  Badge.jsx{sizeStyles[size]}  Badge.jsx{className}`}></div>
  );
};
