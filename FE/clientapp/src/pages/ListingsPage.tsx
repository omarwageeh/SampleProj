import React, { useEffect, useState } from 'react';
import { fetchProducts, createProduct, deleteProduct, Product } from '../api';
import ProductForm from '../components/ProductForm';
import Spinner from '../components/Spinner';
import { useNavigate } from 'react-router-dom';
import './ListingsPage.scss';
import ProductList from '../components/ProductList';
import Modal from '../components/Modal';

const ListingsPage: React.FC = () => {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<Error | null>(null);
  const [showForm, setShowForm] = useState<boolean>(false);
  const [formLoading, setFormLoading] = useState<boolean>(false);
  const [formError, setFormError] = useState<Error | null>(null);
  const [formSuccess, setFormSuccess] = useState<string | null>(null);
  const [deleteSuccess, setDeleteSuccess] = useState<string | null>(null);

  const navigate = useNavigate();

  const load = React.useCallback(() => {
    setLoading(true);
    fetchProducts()
      .then(setProducts)
      .catch(setError)
      .finally(() => setLoading(false));
  }, []);
  useEffect(() => { load(); }, [load]);

  useEffect(() => {
    if (deleteSuccess) {
      const timer = setTimeout(() => setDeleteSuccess(null), 2000);
      return () => clearTimeout(timer);
    }
  }, [deleteSuccess]);

  const handleCreate = async (product: Product) => {
    setFormLoading(true); setFormError(null); setFormSuccess(null);
    try {
      await createProduct(product);
      setFormSuccess('Product created successfully!');
      setTimeout(() => setShowForm(false), 1000);
      load();
    } catch (e: any) { setFormError(e); }
    setFormLoading(false);
  };

  const handleDelete = async (id: string) => {
    if (!window.confirm('Delete this product?')) return;
    try {
      await deleteProduct(id);
      setDeleteSuccess('Product deleted successfully!');
      load();
    } catch (e: any) { alert('Delete failed: ' + e.message); }
  };

  if (loading) return <div style={{ position: 'relative', minHeight: 200 }}><Spinner centered /></div>;
  if (error) return <div className="error">Error: {error.message}</div>;

  return (
    <div>
      <div className="product-list-header">
        <h2>Product Listings</h2>
        <button onClick={() => { setShowForm(f => !f); setFormSuccess(null); setFormError(null); }}>{'Add Product'}</button>
      </div>
      <Modal open={showForm} onClose={() => setShowForm(false)}>
        <ProductForm onSave={handleCreate} loading={formLoading} error={formError} success={formSuccess} />
      </Modal>
      {deleteSuccess && <div className="success">{deleteSuccess}</div>}
      <ProductList
        products={products}
        onView={id => navigate(`/product/${id}`)}
        onDelete={handleDelete}
      />
    </div>
  );
};

export default ListingsPage; 