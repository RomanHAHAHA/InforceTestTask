import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import Swal from 'sweetalert2';
import { API_BASE_URL } from '../apiConfig';

const Login = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [errors, setErrors] = useState({});
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();
  const { fetchUser } = useAuth();

  const handleChange = (e) => {
    const { name, value } = e.target;
    if (name === 'email') setEmail(value);
    if (name === 'password') setPassword(value);
    
    if (errors[name]) {
      setErrors(prev => ({ ...prev, [name]: '' }));
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    
    try {
      const response = await fetch(`${API_BASE_URL}users/login`, {
        method: 'POST',
        credentials: 'include',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password }),
      });

      if (response.ok) {
        await fetchUser();
        setErrors({});
        Swal.fire({
          position: 'center',
          icon: 'success',
          title: 'Logged in successfully',
          showConfirmButton: false,
          timer: 1500
        });
        navigate('/');
      } else if (response.status === 409) {
        const error = await response.json();
        Swal.fire({
          icon: 'error',
          title: 'Login Error',
          text: error.message,
          confirmButtonColor: '#0d6efd'
        });
      } else {
        const error = await response.json();
        
        if (response.status === 400) {
          const validationErrors = {};
          for (const field in error.errors) {
            if (error.errors[field]?.length > 0) {
              validationErrors[field.toLowerCase()] = error.errors[field][0];
            }
          }
          setErrors(validationErrors);
          return;
        }
        
        throw new Error(error.message || 'Login failed');
      }
    } catch (error) {
      Swal.fire({
        icon: 'error',
        title: 'Login Error',
        text: error.message,
        confirmButtonColor: '#0d6efd'
      });
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="container d-flex align-items-center justify-content-center" style={{ minHeight: '80vh' }}>
      <div className="w-100" style={{ maxWidth: '400px' }}>
        <div className="card shadow-sm">
          <div className="card-body p-4">
            <div className="text-center mb-4">
              <h2 className="fw-bold">
                <i className="bi bi-box-arrow-in-right me-2"></i>
                Login
              </h2>
              <p className="text-muted">Please enter your credentials</p>
            </div>

            <form onSubmit={handleSubmit}>
              <div className="form-floating mb-3">
                <input
                  type="text"
                  className={`form-control ${errors.email ? 'is-invalid' : ''}`}
                  id="floatingEmail"
                  name="email"
                  placeholder="name@example.com"
                  value={email}
                  onChange={handleChange}
                />
                <label htmlFor="floatingEmail">Email address</label>
                <i className="bi bi-envelope input-icon"></i>
                {errors.email && <div className="invalid-feedback d-block mt-1">{errors.email}</div>}
              </div>

              <div className="form-floating mb-4">
                <input
                  type="password"
                  className={`form-control ${errors.password ? 'is-invalid' : ''}`}
                  id="floatingPassword"
                  name="password"
                  placeholder="Password"
                  value={password}
                  onChange={handleChange}
                />
                <label htmlFor="floatingPassword">Password</label>
                <i className="bi bi-lock input-icon"></i>
                {errors.password && <div className="invalid-feedback d-block mt-1">{errors.password}</div>}
              </div>

              <button 
                type="submit" 
                className="btn btn-primary w-100 py-2"
                disabled={isLoading}
              >
                {isLoading ? (
                  <>
                    <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                    Logging in...
                  </>
                ) : (
                  <>
                    <i className="bi bi-box-arrow-in-right me-2"></i>
                    Login
                  </>
                )}
              </button>
            </form>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Login;