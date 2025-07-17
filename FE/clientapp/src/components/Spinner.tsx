import React from 'react';
import './Spinner.scss';

const Spinner: React.FC<{ centered?: boolean }> = ({ centered }) => (
  <div className={centered ? 'spinner-centered' : 'spinner'}>Loading...</div>
);

export default Spinner; 