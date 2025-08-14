import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';
import { useAuth } from '../contexts/AuthContext';
import { API_BASE_URL } from '../apiConfig';
import AddUrlForm from './AddUrlForm';

const MySwal = withReactContent(Swal);

const UrlTable = () => {
  const [urls, setUrls] = useState([]);
  const [showAddForm, setShowAddForm] = useState(false);
  const { user } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    fetchUrls();
  }, []);

  const fetchUrls = async () => {
    try {
      const response = await fetch(`${API_BASE_URL}urls`, {
        credentials: 'include'
      });
      const data = await response.json();
      setUrls(data.data);
    } catch (error) {
      MySwal.fire('Error', 'Failed to fetch URLs', 'error');
    }
  };

  const handleUrlAdded = () => {
    fetchUrls();
  };

  const handleDelete = async (id, e) => {
    e.stopPropagation(); 
    
    try {
      const response = await fetch(`${API_BASE_URL}urls/${id}`, {
        method: 'DELETE',
        credentials: 'include'
      });

      if (response.ok){
        setUrls(urls.filter(url => url.id !== id));
        MySwal.fire('Deleted!', 'URL has been deleted.', 'success');
      }
      else {
        const error = await response.json();
        MySwal.fire('Error', error.message, 'error');
      }
    } catch (error) {
      MySwal.fire('Error', 'Failed to delete URL', 'error');
    }
  };

  const handleRowClick = (urlId) => {
    if (user) {
      navigate(`/urls/${urlId}`);
    }
  };

  return (
    <div className="container mt-4">
      <h2>URL Shortener</h2>
      
      {user && (
        <button 
          className="btn btn-primary mb-3"
          onClick={() => setShowAddForm(true)}
          disabled={showAddForm}
        >
          <i className="bi bi-plus-circle me-2"></i>
          Add New URL
        </button>
      )}

      {showAddForm && (
        <AddUrlForm 
          onUrlAdded={handleUrlAdded}
          onCancel={() => setShowAddForm(false)}
        />
      )}

      <table className="table table-striped table-hover">
        <thead>
          <tr>
            <th>Original URL</th>
            <th>Short URL</th>
            <th>CreatedAt</th>
            {user && <th>Actions</th>}
          </tr>
        </thead>
        <tbody>
          {urls.map(url => (
            <tr 
              key={url.id}
              onClick={() => handleRowClick(url.id)}
              style={{ cursor: 'pointer' }}
            >
              <td>
                <a 
                  href={url.originalUrl} 
                  target="_blank" 
                  rel="noopener noreferrer"
                  onClick={(e) => e.stopPropagation()}
                  style={{
                    display: 'inline-block',
                    maxWidth: '300px',       
                    whiteSpace: 'nowrap',    
                    overflow: 'hidden',      
                    textOverflow: 'ellipsis' 
                  }}
                  title={url.originalUrl} 
                >
                  {url.originalUrl}
                </a>
              </td>
              <td>
                <a 
                  href={url.shortUrl}
                  data-type="short-url"
                  rel="noopener noreferrer"
                >
                  {url.shortUrl}
                </a>
              </td>
              <td>{url.createdAt}</td>
              {user && (
                <td>
                  <button
                    className="btn btn-danger"
                    style={{
                      padding: '6px 12px',
                      fontSize: '14px',
                      display: 'flex',
                      alignItems: 'center',
                      gap: '6px'
                    }}
                    onClick={(e) => handleDelete(url.id, e)}
                  >
                    <i className="bi bi-trash"></i>
                    Delete
                  </button>
                </td>
              )}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default UrlTable;