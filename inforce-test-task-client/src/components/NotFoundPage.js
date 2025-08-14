import { Link } from 'react-router-dom';

const NotFoundPage = () => {
  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-50 p-5">
      <div className="text-center max-w-md">
        <h1 className="text-6xl font-bold text-red-600 mb-4">404</h1>
        <h2 className="text-2xl font-semibold text-gray-800 mb-2">
          Oops! Page Not Found
        </h2>
        <p className="text-gray-600 mb-6">
          The page you're looking for doesn't exist or has been moved.
        </p>
        <Link
          to="/"
          className="inline-block px-5 py-2 bg-blue-600 rounded-lg hover:bg-blue-700 transition-colors duration-200 shadow-md hover:shadow-lg font-medium"
        >
          ← Back to Home
        </Link>
      </div>
    </div>
  );
};

export default NotFoundPage;