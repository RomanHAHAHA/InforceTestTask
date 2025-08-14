import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import Swal from 'sweetalert2';
import { API_BASE_URL } from '../apiConfig';

const UrlInfo = () => {
  const { id } = useParams();
  const [urlInfo, setUrlInfo] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchUrlInfo = async () => {
      try {
        const response = await fetch(`${API_BASE_URL}urls/${id}`, {
          credentials: 'include'
        });
        
        if (!response.ok) {
          throw new Error('Failed to fetch URL info');
        }
        
        const data = await response.json();
        setUrlInfo(data.data);
      } catch (error) {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: error.message,
          confirmButtonColor: '#0d6efd'
        });
      } finally {
        setIsLoading(false);
      }
    };

    fetchUrlInfo();
  }, [id]);

  if (isLoading) {
    return (
      <div className="container mt-5 text-center">
        <div className="spinner-border text-primary" role="status">
          <span className="visually-hidden">Loading...</span>
        </div>
        <p className="mt-2">Loading URL information...</p>
      </div>
    );
  }

  if (!urlInfo) {
    return (
      <div className="container mt-5 text-center">
        <div className="alert alert-danger">
          Failed to load URL information
        </div>
      </div>
    );
  }

  return (
    <div className="container mt-4">
      <div className="row">
        <div className="col-md-8">
          <div className="card mb-4 shadow-sm">
            <div className="card-header bg-primary text-white">
              <h5 className="mb-0">
                <i className="bi bi-link-45deg me-2"></i>
                URL Information
              </h5>
            </div>
            <div className="card-body">
              <div className="mb-3">
                <h6 className="text-muted">Original URL</h6>
                <a 
                  href={urlInfo.originalUrl} 
                  target="_blank" 
                  rel="noopener noreferrer"
                  className="text-break"
                >
                  {urlInfo.originalUrl}
                </a>
              </div>

              <div className="mb-3">
                <h6 className="text-muted">Short URL</h6>
                <a 
                  href={urlInfo.shortUrl} 
                  rel="noopener noreferrer"
                  className="text-break"
                >
                  {urlInfo.shortUrl}
                </a>
              </div>

              <div className="mb-3">
                <h6 className="text-muted">Content</h6>
                <p className="card-text">
                  {urlInfo.content || <span className="text-muted">No description provided</span>}
                </p>
              </div>
            </div>
          </div>
        </div>

        <div className="col-md-4">
          <div className="card shadow-sm">
            <div className="card-header bg-info text-white">
              <h5 className="mb-0">
                <i className="bi bi-person-circle me-2"></i>
                Creator Information
              </h5>
            </div>
            <div className="card-body">
              <div className="mb-3">
                <h6 className="text-muted">Nickname</h6>
                <p>{urlInfo.creator.nickName}</p>
              </div>

              <div className="mb-3">
                <h6 className="text-muted">Email</h6>
                <p>{urlInfo.creator.email}</p>
              </div>

              <div className="mb-3">
                <h6 className="text-muted">Role</h6>
                <span className={`badge ${
                  urlInfo.creator.role === 'Admin' ? 'bg-danger' : 'bg-secondary'
                }`}>
                  {urlInfo.creator.role}
                </span>
              </div>

              <div className="mb-3">
                <h6 className="text-muted">Registered Date</h6>
                <p>{urlInfo.creator.registeredDate}</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default UrlInfo;