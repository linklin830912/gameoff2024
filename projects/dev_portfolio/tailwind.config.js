
/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{js,jsx,ts,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        'background-blue': '#0b1621',
        'menu-blue': '#17222e',
        'menu-hover-blue': '#1e2c3b',
        'menu-font': '#faf4be',
        'menu-font-hover': '#ffffff',
        'content-font': '#faf4be',
        'content-sub-font': '#1f2b38',
        'basic': '#697c91',
        'middle': '#faf4be',
        'advanced': '#d4c22a',
      },
      fontFamily: {
        sans: ['Graphik', 'sans-serif'],
        serif: ['Merriweather', 'serif'],
      },
      fontSize: {
        'h0': '240px',
        'h1': '52px',
        'h2': '36px',
        'h3': '24px',
        'h4': '18px',
        'h5': '14px',
        'h6': '12px',
        'h7': '8px',
      },
       screens: {
      'sm': '640px',
      'md': '768px',
      'lg': '1024px',
      'xl': '1280px',
      '2xl': '1536px',
    }
    },
  },
  plugins: [],
}

