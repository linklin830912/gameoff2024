import React from 'react';

import MenuSection from './sections/MenuSection';
import PageSection from './sections/PageSection';
import i18n from './i18n.config';

function App() {
  i18n.changeLanguage('english');
  return (
    <div className="w-[100vw] overflow-x-hidden bg-background-blue flex flex-col items-stretch relative" >       
      <div className='bg-menu-blue w-5 hover:w-auto md:hover:w-[100%] md:w-[100%] md:h-[7vh] overflow-hidden left-0 top-0 h-[100vh] fixed z-30'>
        <MenuSection/>
      </div>
        <div className='flex justify-center pl-6 lg:px-60'>
        <PageSection/>
      </div>
    </div>
  );
}

export default App;
