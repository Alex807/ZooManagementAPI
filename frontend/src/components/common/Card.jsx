// src/components/common/Card.jsx
export const Card = ({ children, className = '', title = null, footer = null, ...props }) => {
  return (
    <div
      className={`bg-white rounded-lg shadow-md overflow-hidden  Textarea.jsx{className}`}
      {...props}
    >
      {title && (
        <div className=`px-6 py-4 border-b border-gray-200 bg-gray-50`>
          <h3 className=`text-lg font-semibold text-gray-800`>{title}</h3>
        </div>
      )}
      <div className=`p-6`>{children}</div>
      {footer && (
        <div className=`px-6 py-4 border-t border-gray-200 bg-gray-50`>
          {footer}
        </div>
      )}
    </div>
  );
};
