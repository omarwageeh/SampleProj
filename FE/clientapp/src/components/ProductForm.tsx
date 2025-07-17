import React, { useEffect, useState } from 'react';
import Spinner from './Spinner';
import { Product } from '../api';
import './ProductForm.scss';

interface ProductFormProps {
  initial?: Product;
  onSave: (product: Product) => void;
  loading: boolean;
  error: Error | null;
  success: string | null;
}

const ProductForm: React.FC<ProductFormProps> = ({ initial, onSave, loading, error, success }) => {
  const [form, setForm] = useState<Product>(initial || { name: '', description: '', price: '', image: null });
  const [touched, setTouched] = useState<Record<string, boolean>>({});
  const [validation, setValidation] = useState<Record<string, string>>({});
  const [preview, setPreview] = useState<string | null>(initial?.imageUrl ? initial.imageUrl : null);

  useEffect(() => {
    setForm(initial || { name: '', description: '', price: '', image: null });
    setPreview(initial?.imageUrl || null);
  }, [initial]);

  useEffect(() => {
    const v: Record<string, string> = {};
    if (!form.name) v.name = 'Name is required';
    if (!form.description) v.description = 'Description is required';
    if (!form.price) v.price = 'Price is required';
    else if (isNaN(Number(form.price)) || Number(form.price) <= 0) v.price = 'Price must be a positive number';
    setValidation(v);
  }, [form]);

  const handleBlur = (field: string) => setTouched(t => ({ ...t, [field]: true }));
  const isValid = Object.keys(validation).length === 0;

  const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0] || null;
    setForm(f => ({ ...f, image: file }));
    if (file) {
      const reader = new FileReader();
      reader.onloadend = () => setPreview(reader.result as string);
      reader.readAsDataURL(file);
    } else {
      setPreview(null);
    }
  };

  return (
    <form className="product-form" onSubmit={e => { e.preventDefault(); if (isValid) onSave(form); }}>
      <div>
        <label>Name: <input value={form.name} onChange={e => setForm(f => ({ ...f, name: e.target.value }))} onBlur={() => handleBlur('name')} required /></label>
        {touched.name && validation.name && <span className="error">{validation.name}</span>}
      </div>
      <div>
        <label>Description: <input value={form.description} onChange={e => setForm(f => ({ ...f, description: e.target.value }))} onBlur={() => handleBlur('description')} required /></label>
        {touched.description && validation.description && <span className="error">{validation.description}</span>}
      </div>
      <div>
        <label>Price: <input type="number" value={form.price} onChange={e => setForm(f => ({ ...f, price: e.target.value }))} onBlur={() => handleBlur('price')} required /></label>
        {touched.price && validation.price && <span className="error">{validation.price}</span>}
      </div>
      <div>
        <label>Image: <input type="file" accept="image/*" onChange={handleImageChange} /></label>
        {preview && <div style={{ marginTop: 8 }}><img src={preview} alt="Preview" style={{ maxWidth: 120, maxHeight: 120, borderRadius: 8 }} /></div>}
      </div>
      <button type="submit" disabled={loading || !isValid}>Save</button>
      {loading && <Spinner />}
      {error && <div className="error">Error: {error.message}</div>}
      {success && <div className="success">{success}</div>}
    </form>
  );
};

export default ProductForm; 