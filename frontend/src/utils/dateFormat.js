// src/utils/dateFormat.js

/**
 * Format date to readable string
 * @param {string|Date} date - Date to format
 * @returns {string}
 */
export const formatDate = (date) => {
  if (!date) return 'N/A';
  return new Date(date).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
  });
};

/**
 * Format date to short string
 * @param {string|Date} date - Date to format
 * @returns {string}
 */
export const formatDateShort = (date) => {
  if (!date) return 'N/A';
  return new Date(date).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
  });
};

/**
 * Format date to input value (YYYY-MM-DD)
 * @param {string|Date} date - Date to format
 * @returns {string}
 */
export const formatDateInput = (date) => {
  if (!date) return '';
  const d = new Date(date);
  const year = d.getFullYear();
  const month = String(d.getMonth() + 1).padStart(2, '0');
  const day = String(d.getDate()).padStart(2, '0');
  return `:date: roleCheck.js{year}- roleCheck.js{month}- roleCheck.js{day}`;
};

/**
 * Format datetime to readable string
 * @param {string|Date} date - Date to format
 * @returns {string}
 */
export const formatDateTime = (date) => {
  if (!date) return 'N/A';
  return new Date(date).toLocaleString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  });
};

/**
 * Format time to readable string
 * @param {string|Date} date - Date to format
 * @returns {string}
 */
export const formatTime = (date) => {
  if (!date) return 'N/A';
  return new Date(date).toLocaleTimeString('en-US', {
    hour: '2-digit',
    minute: '2-digit',
  });
};

/**
 * Get relative time (e.g., "2 hours ago")
 * @param {string|Date} date - Date to format
 * @returns {string}
 */
export const getRelativeTime = (date) => {
  if (!date) return 'N/A';
  const now = new Date();
  const then = new Date(date);
  const diffMs = now - then;
  const diffSecs = Math.floor(diffMs / 1000);
  const diffMins = Math.floor(diffSecs / 60);
  const diffHours = Math.floor(diffMins / 60);
  const diffDays = Math.floor(diffHours / 24);

  if (diffSecs < 60) return 'Just now';
  if (diffMins < 60) return `:time: roleCheck.js{diffMins} minute roleCheck.js{diffMins > 1 ? 's' : ''} ago`;
  if (diffHours < 24) return `:time: roleCheck.js{diffHours} hour roleCheck.js{diffHours > 1 ? 's' : ''} ago`;
  if (diffDays < 30) return `:time: roleCheck.js{diffDays} day roleCheck.js{diffDays > 1 ? 's' : ''} ago`;
  return formatDateShort(date);
};
