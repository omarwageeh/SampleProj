import React from 'react';
import './ProductList.scss';
import { Product } from '../api';
import ProductCard from './ProductCard';

interface ProductListProps {
  products: Product[];
  onView?: (id: string) => void;
  onDelete?: (id: string) => void;
}

const ProductList: React.FC<ProductListProps> = ({ products, onView, onDelete }) => (
  <div className="product-list">
    {products.map(product => (
      <ProductCard
        key={product.id}
        product={product}
        onView={onView}
        onDelete={onDelete}
      />
    ))}
  </div>
);

export default ProductList; 