import React, { FC, useState } from 'react';
import CustomLayout from '../components/layout/layout';
import CrossInhibition from './cross-inhibition/cross-inhibition';
import { ConfigProvider } from 'antd';
import ukUA from 'antd/es/locale/uk_UA';

import './app.scss';

const App: FC = () => {
    const [loading, setLoading] = useState<boolean>(false);

    return (
        <ConfigProvider locale={ukUA}>
            <CustomLayout loading={loading}>
                <CrossInhibition setLoading={setLoading} />
            </CustomLayout>
        </ConfigProvider>
    );
}

export default App;
