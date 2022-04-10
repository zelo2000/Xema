import { FC, useMemo } from 'react';
import { Tag, Table } from 'antd';
import { ColumnType } from 'antd/lib/table';

import { CrossInhibitonIndexCell } from '../../../../types/cross-inhibiton-index-cell';
import { InhibitionColors } from '../../../../types/enums/InhibitionColors';

import './cross-inhibition-grid-item.scss';

interface ICrossInhibitionGridItemProps {
  antigenLabels: string[];
  markedAntigenLabels: string[];
  crossInhibitionIndexes: CrossInhibitonIndexCell[][];
}

const CrossInhibitionGridItem: FC<ICrossInhibitionGridItemProps> = ({
  antigenLabels,
  markedAntigenLabels,
  crossInhibitionIndexes
}: ICrossInhibitionGridItemProps) => {
  const column = useMemo(() => {
    const headerLabels = markedAntigenLabels || [];

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
      fixed: 'left',
      render: (value: CrossInhibitonIndexCell, record: CrossInhibitonIndexCell[], iIndex: number) => {
        return <span style={{ fontWeight: 500 }}>{antigenLabels[iIndex]}</span>
      }
    } as ColumnType<CrossInhibitonIndexCell[]>;

    result.splice(0, 0, labelColumn);
    return result;
  }, [antigenLabels, markedAntigenLabels]);

  return (
    <Table
      className="cross-inhibition-table"
      columns={column}
      dataSource={crossInhibitionIndexes}
      pagination={false}
      scroll={{ x: (markedAntigenLabels.length || 0) * 72 }}
    />
  );
}

export default CrossInhibitionGridItem;