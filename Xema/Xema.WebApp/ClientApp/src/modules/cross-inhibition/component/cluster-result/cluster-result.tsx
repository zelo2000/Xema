import { FC, useEffect, useState } from 'react';
import { Row, Typography, List, Card } from 'antd';
import { toRoman } from 'roman-numerals'
import { Clusters } from '../../../../types/cross-inhibiton-raw-data-model';

import './cluster-result.scss';

interface IClusterResultProps {
  data?: Clusters
}

const ClusterResult: FC<IClusterResultProps> = ({ data }: IClusterResultProps) => {
  const [localClusters, setClusters] = useState<[string, string[]][]>();

  useEffect(() => {
    if (data) {
      setClusters(Object.entries(data));
    }
  }, [data]);

  return (
    <>
      <Typography.Title level={4}>Антитіла по групах</Typography.Title>
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
          renderItem={(item, index) => {
            const groupTitle = Number.isNaN(parseInt(item[0])) ? item[0] : `Group ${toRoman(parseInt(item[0]) + 1)}`;
            return (
              <List.Item key={`card-group-${index}`}>
                <Card
                  title={groupTitle}
                  className="cluster-result-card"
                >
                  {item[1].join(', ')}
                </Card>
              </List.Item>
            );
          }}
        />
      </Row>
    </>
  );
}

export default ClusterResult;