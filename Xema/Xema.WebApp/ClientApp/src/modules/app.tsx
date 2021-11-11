import React, { FC } from 'react';
import CustomLayout from '../components/layout/layout';
import CrossInhibition from './cross-inhibition/cross-inhibition';

import './app.scss';

const App: FC = () => {
  return (
    <CustomLayout>
      <CrossInhibition />
    </CustomLayout>
  );
}

export default App;
