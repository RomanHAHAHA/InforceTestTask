import { useState, useEffect } from 'react';
import { useAuth } from '../contexts/AuthContext';
import Swal from 'sweetalert2';
import { API_BASE_URL } from '../apiConfig';

const About = () => {
  const { user } = useAuth();
  const [isEditing, setIsEditing] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [content, setContent] = useState({
    description: '',
    algorithm: '',
    features: []
  });

  useEffect(() => {
    const fetchContent = async () => {
      try {
        const response = await fetch(`${API_BASE_URL}about-page/content`);
        if (!response.ok) throw new Error('Failed to fetch content');
        
        const data = await response.json();
        setContent({
          description: data.data.description || '',
          algorithm: data.data.algorithm || '',
          features: data.data.features || []
        });
      } catch (error) {
        Swal.fire('Error', error.message, 'error');
      } finally {
        setIsLoading(false);
      }
    };

    fetchContent();
  }, []);

  const handleSave = async () => {
    try {
      setIsLoading(true);
      const response = await fetch(`${API_BASE_URL}about-page`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json', },
        credentials: 'include',
        body: JSON.stringify({
          description: content.description,
          algorithm: content.algorithm,
          features: content.features
        })
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Failed to save changes');
      }

      Swal.fire('Success', 'Content updated successfully!', 'success');
      setIsEditing(false);
    } catch (error) {
      Swal.fire('Error', error.message, 'error');
    } finally {
      setIsLoading(false);
    }
  };

  const handleChange = (field, value) => {
    setContent(prev => ({ ...prev, [field]: value }));
  };

  const handleFeatureChange = (index, value) => {
    const newFeatures = [...content.features];
    newFeatures[index] = value;
    setContent(prev => ({ ...prev, features: newFeatures }));
  };

  const addFeature = () => {
    setContent(prev => ({ ...prev, features: [...prev.features, ''] }));
  };

  const removeFeature = (index) => {
    setContent(prev => ({
      ...prev,
      features: prev.features.filter((_, i) => i !== index)
    }));
  };

  if (isLoading) {
    return (
      <div className="container mt-4 text-center">
        <div className="spinner-border text-primary" role="status">
          <span className="visually-hidden">Loading...</span>
        </div>
      </div>
    );
  }

  return (
    <div className="container mt-4">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h2>About URL Shortener</h2>
        {user?.role === 'Admin' && (
          <button 
            className={`btn ${isEditing ? 'btn-danger' : 'btn-primary'}`}
            onClick={() => setIsEditing(!isEditing)}
            disabled={isLoading}
          >
            {isEditing ? 'Cancel' : 'Edit Content'}
          </button>
        )}
      </div>

      <div className="card">
        <div className="card-body">
          {isEditing ? (
            <>
              <div className="mb-3">
                <label className="form-label">Description</label>
                <textarea
                  className="form-control"
                  rows="4"
                  value={content.description}
                  onChange={(e) => handleChange('description', e.target.value)}
                />
              </div>

              <div className="mb-3">
                <label className="form-label">Algorithm Details</label>
                <textarea
                  className="form-control font-monospace"
                  rows="8"
                  value={content.algorithm}
                  onChange={(e) => handleChange('algorithm', e.target.value)}
                />
              </div>

              <div className="mb-3">
                <label className="form-label">Features</label>
                {content.features.map((feature, index) => (
                  <div key={index} className="input-group mb-2">
                    <input
                      type="text"
                      className="form-control"
                      value={feature}
                      onChange={(e) => handleFeatureChange(index, e.target.value)}
                    />
                    <button
                      className="btn btn-outline-danger"
                      type="button"
                      onClick={() => removeFeature(index)}
                      disabled={isLoading}
                    >
                      <i className="bi bi-trash"></i>
                    </button>
                  </div>
                ))}
                <button 
                  className="btn btn-sm btn-outline-primary mt-2"
                  onClick={addFeature}
                  disabled={isLoading}
                >
                  Add Feature
                </button>
              </div>

              <button 
                className="btn btn-success"
                onClick={handleSave}
                disabled={isLoading}
              >
                {isLoading ? (
                  <>
                    <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                    Saving...
                  </>
                ) : 'Save Changes'}
              </button>
            </>
          ) : (
            <>
              <h5 className="card-title">How it works</h5>
              <p className="card-text">{content.description}</p>
              
              <h5 className="card-title mt-4">Algorithm</h5>
              <div className="bg-light p-3 rounded mb-3">
                <pre className="mb-0">{content.algorithm}</pre>
              </div>

              <h5 className="card-title mt-4">Features</h5>
              <ul>
                {content.features.map((feature, index) => (
                  <li key={index}>{feature}</li>
                ))}
              </ul>
            </>
          )}
        </div>
      </div>
    </div>
  );
};

export default About;