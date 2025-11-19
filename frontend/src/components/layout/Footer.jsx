// src/components/layout/Footer.jsx
export const Footer = () => {
  return (
    <footer className="bg-gray-800 text-white py-6 mt-auto">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex flex-col md:flex-row justify-between items-center">
          <div className="text-center md:text-left mb-4 md:mb-0">
            <p className="text-sm"> 2025 Zoo Management System. All rights reserved.</p>
          </div>
          <div className="flex space-x-6">
            <a href="#" className="text-sm hover:text-green-400 transition-colors">
              About
            </a>
            <a href="#" className="text-sm hover:text-green-400 transition-colors">
              Contact
            </a>
            <a href="#" className="text-sm hover:text-green-400 transition-colors">
              Privacy Policy
            </a>
          </div>
        </div>
      </div>
    </footer>
  );
};

