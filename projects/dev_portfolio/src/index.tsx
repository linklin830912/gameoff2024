import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter } from "react-router-dom";
import './index.css';
import App from './App';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
const rootStyles = getComputedStyle(document.documentElement);

export const backgroundBlueColor = '#0b1621';
export const menuBlueColor = '#17222e';
export const menuHoverBlueColor = '#1e2c3b';
export const menuFontColor = '#faf4be';
export const menuFontHoverColor = '#ffffff';
export const contentFontColor = "#faf4be";
export const contentSubFontColor = '#1f2b38';
export const basicColor = '#697c91';
export const middleColor = '#fcf18d';
export const advancedColor = '#d4c22a';

root.render(
  <React.StrictMode>
    <BrowserRouter>
      <App />
    </BrowserRouter>
  </React.StrictMode>
);
