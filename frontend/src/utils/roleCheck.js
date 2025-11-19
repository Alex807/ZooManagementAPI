// src/utils/roleCheck.js

// Role hierarchy (higher number = higher privilege)
export const roleHierarchy = {
  Admin: 4,
  Zookeeper: 3,
  Veterinarian: 2,
  Visitor: 1,
};

/**
 * Check if user has required role or higher
 * @param {string} userRole - Current user's role
 * @param {string} requiredRole - Required role for access
 * @returns {boolean}
 */
export const hasRole = (userRole, requiredRole) => {
  if (!userRole || !requiredRole) return false;
  return (roleHierarchy[userRole] || 0) >= (roleHierarchy[requiredRole] || 0);
};

/**
 * Check if user has exact role
 * @param {string} userRole - Current user's role
 * @param {string} targetRole - Target role to check
 * @returns {boolean}
 */
export const isRole = (userRole, targetRole) => {
  return userRole === targetRole;
};

/**
 * Get role level
 * @param {string} role - Role name
 * @returns {number}
 */
export const getRoleLevel = (role) => {
  return roleHierarchy[role] || 0;
};

/**
 * Check if user is admin
 * @param {string} userRole - Current user's role
 * @returns {boolean}
 */
export const isAdmin = (userRole) => {
  return userRole === 'Admin';
};

/**
 * Check if user is staff (Zookeeper or Veterinarian or Admin)
 * @param {string} userRole - Current user's role
 * @returns {boolean}
 */
export const isStaff = (userRole) => {
  return hasRole(userRole, 'Veterinarian');
};

/**
 * Check if user has StaffOrAbove privilege
 * @param {string} userRole - Current user's role
 * @returns {boolean}
 */
export const isStaffOrAbove = (userRole) => {
  return ['Admin', 'Zookeeper', 'Veterinarian'].includes(userRole);
};

/**
 * Check if user has ZookeeperOrAbove privilege
 * @param {string} userRole - Current user's role
 * @returns {boolean}
 */
export const isZookeeperOrAbove = (userRole) => {
  return hasRole(userRole, 'Zookeeper');
};

/**
 * Check if user has VeterinarianOrAbove privilege
 * @param {string} userRole - Current user's role
 * @returns {boolean}
 */
export const isVeterinarianOrAbove = (userRole) => {
  return hasRole(userRole, 'Veterinarian');
};

