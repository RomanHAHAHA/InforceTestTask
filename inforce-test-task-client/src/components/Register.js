import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Swal from 'sweetalert2';
import { API_BASE_URL } from '../apiConfig';

const Register = () => {
  const [formData, setFormData] = useState({
    nickName: '',
    email: '',
    password: '',
    passwordConfirm: '',
    isAdmin: false
  });
  const [errors, setErrors] = useState({});
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    setIsLoading(true);
    
    try {
      const response = await fetch(`${API_BASE_URL}users/register`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          nickName: formData.nickName,
          email: formData.email,
          password: formData.password,
          passwordConfirm: formData.passwordConfirm,
          isAdmin: formData.isAdmin
        }),
      });

      if (response.ok) {
        setErrors({});
        Swal.fire({
          position: 'center',
          icon: 'success',
          title: 'Registration successful!',
          text: 'You can now login with your credentials',
          showConfirmButton: false,
          timer: 2000
        });
        navigate('/login');
      } else {
        const error = await response.json();
        
        if (response.status === 400) {
          const validationErrors = {};
          for (const field in error.errors) {
            if (error.errors[field]?.length > 0) {
              const fieldName = field === 'passwordconfirm' ? 'passwordConfirm' : field.toLowerCase();
              validationErrors[fieldName] = error.errors[field][0];
            }
          }
          setErrors(validationErrors);
          return;
        }
        
        throw new Error(error.message || 'Registration failed');
      }
    } catch (error) {
      Swal.fire({
        icon: 'error',
        title: 'Registration Error',
        text: error.message,
        confirmButtonColor: '#0d6efd'
      });
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="container d-flex align-items-center justify-content-center" style={{ minHeight: '80vh' }}>
      <div className="w-100" style={{ maxWidth: '500px' }}>
        <div className="card shadow-sm">
          <div className="card-body p-4">
            <div className="text-center mb-4">
              <h2 className="fw-bold">
                <i className="bi bi-person-plus me-2"></i>
                Create Account
              </h2>
              <p className="text-muted">Please fill in the registration form</p>
            </div>

            <form onSubmit={handleSubmit}>
              <div className="form-floating mb-3">
                <input
                  type="text"
                  className={`form-control ${errors.nickname ? 'is-invalid' : ''}`}
                  id="floatingNickName"
                  name="nickName"
                  placeholder="Nickname"
                  value={formData.nickName}
                  onChange={handleChange}
                />
                <label htmlFor="floatingNickName">Nickname</label>
                <i className="bi bi-person-circle input-icon"></i>
                {errors.nickname && <div className="invalid-feedback d-block mt-1">{errors.nickname}</div>}
              </div>

              <div className="form-floating mb-3">
                <input
                  type="text"
                  className={`form-control ${errors.email ? 'is-invalid' : ''}`}
                  id="floatingEmail"
                  name="email"
                  placeholder="name@example.com"
                  value={formData.email}
                  onChange={handleChange}
                />
                <label htmlFor="floatingEmail">Email address</label>
                <i className="bi bi-envelope input-icon"></i>
                {errors.email && <div className="invalid-feedback d-block mt-1">{errors.email}</div>}
              </div>

              <div className="form-floating mb-3">
                <input
                  type="password"
                  className={`form-control ${errors.password ? 'is-invalid' : ''}`}
                  id="floatingPassword"
                  name="password"
                  placeholder="Password"
                  value={formData.password}
                  onChange={handleChange}
                />
                <label htmlFor="floatingPassword">Password</label>
                <i className="bi bi-lock input-icon"></i>
                {errors.password && <div className="invalid-feedback d-block mt-1">{errors.password}</div>}
              </div>

              <div className="form-floating mb-4">
                <input
                  type="password"
                  className={`form-control ${errors.passwordConfirm ? 'is-invalid' : ''}`}
                  id="floatingPasswordConfirm"
                  name="passwordConfirm"
                  placeholder="Confirm Password"
                  value={formData.passwordConfirm}
                  onChange={handleChange}
                />
                <label htmlFor="floatingPasswordConfirm">Confirm Password</label>
                <i className="bi bi-lock-fill input-icon"></i>
                {errors.passwordconfirm && <div className="invalid-feedback d-block mt-1">{errors.passwordconfirm}</div>}
              </div>

              <div className="form-check mb-4">
                <input
                  className="form-check-input"
                  type="checkbox"
                  id="isAdminCheck"
                  name="isAdmin"
                  checked={formData.isAdmin}
                  onChange={handleChange}
                />
                <label className="form-check-label" htmlFor="isAdminCheck">
                  Register as admin
                </label>
              </div>

              <button 
                type="submit" 
                className="btn btn-primary w-100 py-2 mb-3"
                disabled={isLoading}
              >
                {isLoading ? (
                  <>
                    <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                    Registering...
                  </>
                ) : (
                  <>
                    <i className="bi bi-person-plus me-2"></i>
                    Register
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

export default Register;