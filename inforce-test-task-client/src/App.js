import { Routes, Route } from 'react-router-dom';
import UrlTable from './components/UrlTable';
import Login from './components/Login';
import About from './components/About';
import UrlInfo from './components/UrlInfo';
import Navbar from './components/Navbar';
import Register from './components/Register';
import UrlRedirect from './components/UrlRedirect';
import NotFoundPage from './components/NotFoundPage';

function App() {
  return (
    <>
      <Navbar />
      <div className="container">
        <Routes>
          <Route path="/" element={<UrlTable />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/about" element={<About />} />
          <Route path="/urls/:id" element={<UrlInfo />} />
          <Route path="/:shortCode" element={<UrlRedirect />} />
          <Route path="/not-found" element={<NotFoundPage />} />
        </Routes>
      </div>
    </>
  );
}

export default App;