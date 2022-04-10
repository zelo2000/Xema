import React, { FC, useState } from 'react';
import CustomLayout from '../components/layout/layout';
import CrossInhibition from './cross-inhibition/cross-inhibition';

import './app.scss';

const App: FC = () => {
  const [loading, setLoading] = useState<boolean>(false);

  return (
    <CustomLayout loading={loading}>
      <CrossInhibition setLoading={setLoading} />
    </CustomLayout>
  );
}

export default App;
