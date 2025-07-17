import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { fetchProductById, updateProduct, Product } from '../api';
import ProductForm from '../components/ProductForm';
import Spinner from '../components/Spinner';
import './DetailsPage.scss';

const DetailsPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [product, setProduct] = useState<Product | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<Error | null>(null);
  const [editMode, setEditMode] = useState<boolean>(false);
  const [formLoading, setFormLoading] = useState<boolean>(false);
  const [formError, setFormError] = useState<Error | null>(null);
  const [formSuccess, setFormSuccess] = useState<string | null>(null);

  const load = () => {
    setLoading(true);
    fetchProductById(id!)
      .then(setProduct)
      .catch(setError)
      .finally(() => setLoading(false));
  };
  useEffect(() => { load(); }, [id]);

  const handleUpdate = async (updated: Product) => {
    setFormLoading(true); setFormError(null); setFormSuccess(null);
    try {
      await updateProduct(id!, updated);
      setFormSuccess('Product updated successfully!');
      setTimeout(() => setEditMode(false), 1000);
      load();
    } catch (e: any) { setFormError(e); }
    setFormLoading(false);
  };

  if (loading) return <Spinner />;
  if (error) return <div className="error">Error: {error.message}</div>;
  if (!product) return <div>Product not found</div>;

  return (
    <div className="product-details-page">
      <div style={{ marginBottom: '1.5rem' }}>
        <Link to="/">
          <button type="button" className="back-btn">← Back to Listings</button>
        </Link>
      </div>
      {product.imageUrl && <img src={product.imageUrl} alt={product.name} style={{ width: '100%', maxWidth: 420, maxHeight: 260, objectFit: 'cover', borderRadius: 12, marginBottom: 24 }} />}
      <h2>Product Details</h2>
      {editMode ? (
        <ProductForm initial={product} onSave={handleUpdate} loading={formLoading} error={formError} success={formSuccess} />
      ) : (
        <>
          <p><strong>Name:</strong> {product.name}</p>
          <p><strong>Description:</strong> {product.description}</p>
          <p><strong>Price:</strong> {product.price}</p>
          <button onClick={() => setEditMode(true)}>Edit</button>
        </>
      )}
    </div>
  );
};

export default DetailsPage; 