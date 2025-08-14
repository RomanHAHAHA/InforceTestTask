import { useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { API_BASE_URL } from '../apiConfig';

const UrlRedirect = () => {
  const { shortCode } = useParams();
  const navigate = useNavigate();

  useEffect(() => {
    const fetchOriginalUrl = async () => {
      try {
        const response = await fetch(`${API_BASE_URL}urls/${shortCode}`);
        
        if (!response.ok) {
          throw new Error('URL not found');
        }

        const data = await response.json();
        if (data.data) {          
          window.location.href = data.data;
        } else {
          navigate('/not-found');
        }
      } catch (error) {
        console.error('Redirect error:', error);
        navigate('/not-found');
      }
    };

    if (shortCode) {
      fetchOriginalUrl();
    } else {
      navigate('/');
    }
  }, [shortCode, navigate]);

  return (
    <div className="container mt-5 text-center">
      <div className="spinner-border text-primary" role="status">
        <span className="visually-hidden">Loading...</span>
      </div>
      <p className="mt-3">Redirecting to your destination...</p>
    </div>
  );
};

export default UrlRedirect;