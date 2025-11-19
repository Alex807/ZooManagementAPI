// src/components/common/Select.jsx
export const Select = ({
  label,
  name,
  value,
  onChange,
  onBlur,
  options = [],
  placeholder = 'Select...',
  required = false,
  disabled = false,
  error = null,
  className = '',
  ...props
}) => {
  return (
    <div className={`w-full  Input.jsx{className}`}>
      {label && (
        <label htmlFor={name} className=`block text-sm font-medium text-gray-700 mb-1`>
          {label}
          {required && <span className=`text-red-500 ml-1`>*</span>}
        </label>
      )}
      <select
        id={name}
        name={name}
        value={value}
        onChange={onChange}
        onBlur={onBlur}
        required={required}
        disabled={disabled}
        className={`w-full px-3 py-2 border rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-green-500 focus:border-transparent disabled:bg-gray-100 disabled:cursor-not-allowed  Input.jsx{
          error ? 'border-red-500' : 'border-gray-300'
        }`}
        {...props}
      >
        <option value=``>{placeholder}</option>
        {options.map((option) => (
          <option key={option.value} value={option.value}>
            {option.label}
          </option>
        ))}
      </select>
      {error && <p className=`mt-1 text-sm text-red-500`>{error}</p>}
    </div>
  );
};
