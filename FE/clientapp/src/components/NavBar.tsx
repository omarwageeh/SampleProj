import React from 'react';
import { Link } from 'react-router-dom';

interface NavBarProps {
  darkMode: boolean;
  setDarkMode: React.Dispatch<React.SetStateAction<boolean>>;
}

const NavBar: React.FC<NavBarProps> = ({ darkMode, setDarkMode }) => (
  <nav>
    <div className="nav-inner">
      <Link to="/">Products</Link>
      <button
        style={{ marginLeft: 'auto', background: 'none', border: 'none', color: 'var(--color-primary)', fontWeight: 600, fontSize: '1.1rem', cursor: 'pointer' }}
        onClick={() => setDarkMode(d => !d)}
        aria-label="Toggle dark mode"
      >
        {darkMode ? '🌙 Dark' : '☀️ Light'}
      </button>
    </div>
  </nav>
);

export default NavBar; 