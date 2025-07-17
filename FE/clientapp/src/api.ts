const API_BASE = 'https://localhost:7014/api/products';

export interface Product {
  id?: string;
  name: string;
  description: string;
  price: number | string;
  imageUrl?: string;
  image?: File | null;
}

export async function fetchProducts(): Promise<Product[]> {
  const res = await fetch(API_BASE);
  if (!res.ok) throw new Error('Failed to fetch products');
  return res.json();
}

export async function fetchProductById(id: string): Promise<Product> {
  const res = await fetch(`${API_BASE}/${id}`);
  if (!res.ok) throw new Error('Failed to fetch product');
  return res.json();
}

export async function createProduct(product: Product): Promise<Product> {
  const formData = new FormData();
  formData.append('name', product.name);
  formData.append('description', product.description);
  formData.append('price', String(product.price));
  if (product.image) {
    formData.append('image', product.image);
  }
  const res = await fetch(API_BASE, {
    method: 'POST',
    body: formData,
  });
  if (!res.ok) throw new Error('Failed to create product');
  return res.json();
}

export async function updateProduct(id: string, product: Product): Promise<Response> {
  const formData = new FormData();
  formData.append('id', String(id));
  formData.append('name', product.name);
  formData.append('description', product.description);
  formData.append('price', String(product.price));
  if (product.image) {
    formData.append('image', product.image);
  }
  if (product.imageUrl) {
    formData.append('imageUrl', product.imageUrl);
  }
  const res = await fetch(`${API_BASE}/${id}`, {
    method: 'PUT',
    body: formData,
  });
  if (!res.ok) throw new Error('Failed to update product');
  return res;
}

export async function deleteProduct(id: string): Promise<Response> {
  const res = await fetch(`${API_BASE}/${id}`, {
    method: 'DELETE',
  });
  if (!res.ok) throw new Error('Failed to delete product');
  return res;
} 