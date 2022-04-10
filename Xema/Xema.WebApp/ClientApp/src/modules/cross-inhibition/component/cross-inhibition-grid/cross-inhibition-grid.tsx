import { FC, useEffect, useState } from 'react';
import { Row, Typography } from 'antd';

import { CrossInhibitonRawDataModel } from '../../../../types/cross-inhibiton-raw-data-model';
import CrossInhibitionGridItem from '../cross-inhibition-grid-item/cross-inhibition-grid-item';

import './cross-inhibition-grid.scss';

interface ICrossInhibitionGridProps {
  data?: CrossInhibitonRawDataModel
}

const CrossInhibitionGrid: FC<ICrossInhibitionGridProps> = ({ data }: ICrossInhibitionGridProps) => {
  const [parseResult, setParsedResult] = useState<CrossInhibitonRawDataModel>();

  useEffect(() => {
    setParsedResult(data);
  }, [data]);

  return (
    <>
      <Typography.Title level={4}>Cross inhibition data</Typography.Title>
      {parseResult?.crossInhibitionIndexes.map(crossInhibitionIndex => (
        <Row justify="center" className='cross-inhibition-grid-item-container'>
          <CrossInhibitionGridItem
            crossInhibitionIndexes={crossInhibitionIndex}
            antigenLabels={parseResult?.antigenLabels}
            markedAntigenLabels={parseResult?.markedAntigenLabels}
          />
        </Row>
      ))
      }
    </>
  );
}

export default CrossInhibitionGrid;