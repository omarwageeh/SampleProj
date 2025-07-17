import React from 'react';
import './ProductCard.scss';
import { Product } from '../api';

interface ProductCardProps {
  product: Product;
  onView?: (id: string) => void;
  onDelete?: (id: string) => void;
  children?: React.ReactNode;
}

const ProductCard: React.FC<ProductCardProps> = ({ product, onView, onDelete, children }) => (
  <div className="product-card">
    {product.imageUrl && (
      <img
        src={product.imageUrl}
        alt={product.name}
        style={{ width: '100%', maxHeight: 140, objectFit: 'cover', borderRadius: 8, marginBottom: 8 }}
      />
    )}
    <h3>{product.name}</h3>
    <p className="product-desc">{product.description}</p>
    <p><strong>Price:</strong> {product.price}</p>
    <div className="card-actions">
      {onView && (
        <button onClick={() => onView(product.id!)}>View</button>
      )}
      {onDelete && (
        <button onClick={() => onDelete(product.id!)}>Delete</button>
      )}
      {children}
    </div>
  </div>
);

export default ProductCard; 