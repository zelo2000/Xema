import React, { FC, useState } from 'react';
import CustomLayout from '../components/layout/layout';
import CrossInhibition from './cross-inhibition/cross-inhibition';

import './app.scss';
import { Spin } from 'antd';

const App: FC = () => {
  const [loading, setLoading] = useState<boolean>(false);

  return (
    <CustomLayout>
      <Spin spinning={loading} size="large">
        <CrossInhibition setLoading={setLoading} />
      </Spin>
    </CustomLayout>
  );
}

export default App;
