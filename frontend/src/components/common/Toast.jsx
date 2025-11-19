// src/components/common/Toast.jsx
import { useEffect, useState } from 'react';

export const Toast = ({ message, type = 'info', duration = 3000, onClose }) => {
  const [isVisible, setIsVisible] = useState(true);

  useEffect(() => {
    const timer = setTimeout(() => {
      setIsVisible(false);
      if (onClose) onClose();
    }, duration);

    return () => clearTimeout(timer);
  }, [duration, onClose]);

  if (!isVisible) return null;

  const typeStyles = {
    success: 'bg-green-500',
    error: 'bg-red-500',
    warning: 'bg-amber-500',
    info: 'bg-blue-500',
  };

  return (
    <div className={"fixed bottom-4 right-4 z-50  Table.jsx{typeStyles[type]} text-white px-6 py-3 rounded-lg shadow-lg flex items-center space-x-2 animate-slide-up"}>
      <span>{message}</span>
      <button
        onClick={() => {
          setIsVisible(false);
          if (onClose) onClose();
        }}
        className="ml-4 text-white hover:text-gray-200"
      >
        
      </button>
    </div>
  );
};

