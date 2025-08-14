import { useState } from 'react';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';
import { API_BASE_URL } from '../apiConfig';

const MySwal = withReactContent(Swal);

const AddUrlForm = ({ onUrlAdded, onCancel }) => {
  const [formData, setFormData] = useState({
    url: '',
    description: ''
  });
  const [errors, setErrors] = useState({});
  const [isLoading, setIsLoading] = useState(false);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
    if (errors[name]) {
      setErrors(prev => ({ ...prev, [name]: '' }));
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    
    try {
      const response = await fetch(`${API_BASE_URL}urls`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          url: formData.url,
          description: formData.description
        }),
        credentials: 'include'
      });

      if (!response.ok) {
        const errorData = await response.json();
        
        if (response.status === 400) {
          const validationErrors = {};
          for (const field in errorData.errors) {
            if (errorData.errors[field]?.length > 0) {
              validationErrors[field.toLowerCase()] = errorData.errors[field][0];
            }
          }
          setErrors(validationErrors);
          return;
        }
        
        throw new Error(errorData.message || 'Failed to add URL');
      }

      const addedUrl = await response.json();
      onUrlAdded(addedUrl.data);
      setFormData({ url: '', description: '' });
      setErrors({});
      MySwal.fire('Success', 'URL added successfully', 'success');
    } catch (error) {
      MySwal.fire('Error', error.message, 'error');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="card mb-4">
      <div className="card-body">
        <h5 className="card-title">Add New URL</h5>
        <form onSubmit={handleSubmit}>
          <div className="mb-3">
            <label htmlFor="url" className="form-label">URL to shorten</label>
            <input
              type="text"
              className={`form-control ${errors.url ? 'is-invalid' : ''}`}
              id="url"
              name="url"
              placeholder="https://example.com"
              value={formData.url}
              onChange={handleChange}
            />
            {errors.url && <div className="invalid-feedback d-block mt-1">{errors.url}</div>}
          </div>
          <div className="mb-3">
            <label htmlFor="description" className="form-label">Description</label>
            <textarea
              className={`form-control ${errors.description ? 'is-invalid' : ''}`}
              id="description"
              name="description"
              placeholder="Optional description"
              value={formData.description}
              onChange={handleChange}
              rows="2"
            />
            {errors.description && <div className="invalid-feedback d-block mt-1">{errors.description}</div>}
          </div>
          <div className="d-flex justify-content-between">
            <button 
              type="button" 
              className="btn btn-secondary"
              onClick={onCancel}
              disabled={isLoading}
            >
              Cancel
            </button>
            <button 
              type="submit" 
              className="btn btn-success"
              disabled={isLoading}
            >
              {isLoading ? (
                <>
                  <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                  Adding...
                </>
              ) : 'Shorten URL'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default AddUrlForm;