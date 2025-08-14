import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';
import Swal from 'sweetalert2';

const Navbar = () => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = async () => {
    await logout();
    navigate('/login');
    Swal.fire({
      position: 'top-end',
      icon: 'success',
      title: 'Logged out successfully',
      showConfirmButton: false,
      timer: 1500,
      toast: true
    });
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-primary shadow-sm mb-4">
      <div className="container">
        <Link className="navbar-brand fw-bold" to="/">
          <i className="bi bi-link-45deg me-2"></i>
          URL Shortener
        </Link>
        
        <button 
          className="navbar-toggler" 
          type="button" 
          data-bs-toggle="collapse" 
          data-bs-target="#navbarContent"
        >
          <span className="navbar-toggler-icon"></span>
        </button>
        
        <div className="collapse navbar-collapse" id="navbarContent">
          <ul className="navbar-nav me-auto mb-2 mb-lg-0">
            <li className="nav-item">
              <Link className="nav-link active" to="/">Home</Link>
            </li>
            <li className="nav-item">
              <Link className="nav-link" to="/about">About</Link>
            </li>
          </ul>
          
          <div className="d-flex align-items-center">
            {user ? (
              <>
                {/* Добавленная кнопка Logout */}
                <button 
                  className="btn btn-outline-light me-2" 
                  onClick={handleLogout}
                >
                  <i className="bi bi-box-arrow-right me-1"></i>
                  Logout
                </button>
                
                {/* Существующее dropdown меню */}
                <div className="dropdown">
                  <button 
                    className="btn btn-outline-light dropdown-toggle" 
                    type="button" 
                    id="userDropdown"
                    data-bs-toggle="dropdown"
                  >
                    <i className="bi bi-person-circle me-1"></i>
                    {user.nickName}
                  </button>
                  <ul className="dropdown-menu dropdown-menu-end">
                    <li>
                      <span className="dropdown-item-text text-muted small">
                        <i className="bi bi-shield-fill-check me-1"></i>
                        {user.role}
                      </span>
                    </li>
                    <li><hr className="dropdown-divider"/></li>
                    {user.role === 'Admin' && (
                      <li>
                        <Link className="dropdown-item" to="/admin">
                          <i className="bi bi-speedometer2 me-2"></i>
                          Admin Panel
                        </Link>
                      </li>
                    )}
                    <li>
                      <button 
                        className="dropdown-item text-danger" 
                        onClick={handleLogout}
                      >
                        <i className="bi bi-box-arrow-right me-2"></i>
                        Logout
                      </button>
                    </li>
                  </ul>
                </div>
              </>
            ) : (
              <>
                <Link 
                  className="btn btn-outline-light me-2" 
                  to="/login"
                >
                  <i className="bi bi-box-arrow-in-right me-1"></i>
                  Login
                </Link>
                <Link 
                  className="btn btn-light" 
                  to="/register"
                >
                  <i className="bi bi-person-plus me-1"></i>
                  Register
                </Link>
              </>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;