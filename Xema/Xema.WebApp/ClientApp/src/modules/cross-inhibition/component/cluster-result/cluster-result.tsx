import { FC, useEffect, useState } from 'react';
import { Row, Typography, List, Card } from 'antd';
import { toRoman } from 'roman-numerals'
import { Clusters } from '../../../../types/cross-inhibiton-raw-data-model';

import './cluster-result.scss';

interface IClusterResultProps {
  data?: Clusters
}

const ClusterResult: FC<IClusterResultProps> = ({ data }: IClusterResultProps) => {
  const [localClusters, setClusters] = useState<string[][]>();

  useEffect(() => {
    if (data) {
      setClusters(Object.values(data));
    }
  }, [data]);

  return (
    <>
      <Typography.Title level={4}>Antigen by groups</Typography.Title>
      <Row>
        <List
          grid={{
            gutter: 16,
            xs: 1,
            sm: 2,
            md: 4,
            lg: 4,
            xl: 6,
            xxl: 3,
          }}
          dataSource={localClusters}
          renderItem={(item, index) => (
            <List.Item>
              <Card
                title={`Group ${toRoman(index + 1)}`}
                className="cluster-result-card"
              >
                {item.join(', ')}
              </Card>
            </List.Item>
          )}
        />
      </Row>
    </>
  );
}

export default ClusterResult;