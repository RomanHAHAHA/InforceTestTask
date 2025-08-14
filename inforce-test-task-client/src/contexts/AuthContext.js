import { createContext, useContext, useState, useEffect } from "react";
import { API_BASE_URL } from "../apiConfig"; 
import Swal from "sweetalert2";

const AuthContext = createContext(null);
const claimsUrl = `${API_BASE_URL}users/get-claims`;
const logoutUrl = `${API_BASE_URL}users/logout`;

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);

  const fetchUser = async () => {
    try {
      const response = await fetch(claimsUrl, {
        method: "GET",
        credentials: "include",
      });

      if (response.ok) {
        const data = await response.json();
        setUser({
          userId: data.userCookiesData.userId,
          nickName: data.userCookiesData.nickName,
          role: data.userCookiesData.role,
        });
      } else {
        setUser(null);
      }
    } catch (error) {
      setUser(null);
    }
  };

  const logout = async () => {
    try {
      const response = await fetch(`${logoutUrl}`, { method: "DELETE", credentials: 'include' });
      if (!response.ok) {
        console.log("Logout error");
      }
    } catch (error) {
      Swal.fire({
        icon: 'error',
        title: 'Error',
        text: 'Unexpected error occured during the request'
      })
    }
    setUser(null); 
  };

  useEffect(() => {
    fetchUser(); 
  }, []);

  return (
    <AuthContext.Provider value={{ user, setUser, logout, fetchUser }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);