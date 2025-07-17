import React, { JSX, useEffect, useState } from 'react';
import { BrowserRouter as Router, Routes, Route, Link, useParams } from 'react-router-dom';
import { fetchProducts, fetchProductById, createProduct, updateProduct, deleteProduct, Product } from './api';
import './App.scss';
import ListingsPage from './pages/ListingsPage';
import DetailsPage from './pages/DetailsPage';
import NavBar from './components/NavBar';

function App(): JSX.Element {
  const [darkMode, setDarkMode] = React.useState(() => {
    return localStorage.getItem('theme') === 'dark';
  });

  React.useEffect(() => {
    if (darkMode) {
      document.body.classList.add('dark-mode');
      localStorage.setItem('theme', 'dark');
    } else {
      document.body.classList.remove('dark-mode');
      localStorage.setItem('theme', 'light');
    }
  }, [darkMode]);

  return (
    <Router>
      <NavBar darkMode={darkMode} setDarkMode={setDarkMode} />
      <Routes>
        <Route path="/" element={<ListingsPage />} />
        <Route path="/product/:id" element={<DetailsPage />} />
      </Routes>
    </Router>
  );
}

export default App;
