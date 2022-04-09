import { FC, useEffect, useMemo, useState } from 'react';
import { Row, Typography, Tag, Table } from 'antd';
import { ColumnType } from 'antd/lib/table';

import { CrossInhibitonRawDataModel } from '../../../../types/cross-inhibiton-raw-data-model';
import { CrossInhibitonIndexCell } from '../../../../types/cross-inhibiton-index-cell';
import { InhibitionColors } from '../../../../types/enums/InhibitionColors';

import './cross-inhibition-grid.scss';

interface ICrossInhibitionGridProps {
  data?: CrossInhibitonRawDataModel
}

const CrossInhibitionGrid: FC<ICrossInhibitionGridProps> = ({ data }: ICrossInhibitionGridProps) => {
  const [parseResult, setParsedResult] = useState<CrossInhibitonRawDataModel>();

  useEffect(() => {
    setParsedResult(data);
  }, [data]);

  const column = useMemo(() => {
    const headerLabels = parseResult?.markedAntigenLabels || [];

    const result = headerLabels.map((label, jIndex) => {
      return {
        title: label,
        dataIndex: jIndex,
        render: (value: CrossInhibitonIndexCell, _: CrossInhibitonIndexCell[], iIndex: number) => {
          const color = value?.markerColor === InhibitionColors.DarkGreen
            ? "#00b04f"
            : (value?.markerColor === InhibitionColors.LightGreen ? "#92d050" : undefined);

          return (
            <Tag
              className="index-tag"
              key={`${iIndex}-${jIndex}`}
              color={color}
            >
              {value?.value}
            </Tag>
          );
        }
      } as ColumnType<CrossInhibitonIndexCell[]>;
    });

    const labelColumn = {
      title: 'Label',
      key: 'label',
      render: (value: CrossInhibitonIndexCell, record: CrossInhibitonIndexCell[], iIndex: number) => {
        return <span style={{ fontWeight: 500 }}>{parseResult?.antigenLabels[iIndex]}</span>
      }
    };

    result.splice(0, 0, labelColumn);
    return result;
  }, [parseResult]);

  return (
    <>
      <Typography.Title level={4}>Cross inhibition data</Typography.Title>
      <Row justify="center">
        {parseResult
          ? <Table
            className="cross-inhibition-table"
            columns={column}
            dataSource={parseResult.crossInhibitionIndexes}
            pagination={false}
            scroll={{ x: 1200 }}
          /> : <></>
        }
      </Row>
    </>
  );
}

export default CrossInhibitionGrid;