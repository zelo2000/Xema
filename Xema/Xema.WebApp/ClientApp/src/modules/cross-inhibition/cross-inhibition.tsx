import React, { FC, useCallback, useMemo, useState } from 'react';
import { Button, Upload, Col, Row, message, Typography, Tag, Table, Empty, Tree, Spin } from 'antd';
import { RcFile } from 'antd/lib/upload';
import { ColumnType } from 'antd/lib/table';
import { UploadOutlined } from '@ant-design/icons';

import { uploadFileApi } from '../../api/cross-inhibition-api';
import { CrossInhibitonRawDataModel } from './../../types/cross-inhibiton-raw-data-model';
import { CrossInhibitonIndexCell } from '../../types/cross-inhibiton-index-cell';
import { InhibitionColors } from '../../types/enums/InhibitionColors';

import './cross-inhibition.scss';
import { DataNode } from 'antd/lib/tree';

const CrossInhibition: FC = () => {
  const [parseResult, setParsedResult] = useState<CrossInhibitonRawDataModel>();
  const [clusters, setClusters] = useState<DataNode[]>();
  const [loading, setLoading] = useState<boolean>(false);

  const handleSubmit = useCallback((value) => {
    setLoading(true);
    const formData = new FormData();
    formData.append('file', value.file);

    uploadFileApi(formData).then(responce => {
      setParsedResult(responce);
      const clusters = mapClusters(responce.clusters);
      setClusters(clusters);
      value.onSuccess();
      setLoading(false);
    }).catch((error) => {
      value.onError(error);
      setLoading(false);
    });
  }, []);

  const mapClusters = (clusters: { [key: number]: string[] }) => {
    const result = Object.keys(clusters).map(key => {
      const intKey = parseInt(key, 10)
      return {
        title: `Group ${intKey + 1}`,
        key: key,
        children: [{
          title: clusters[intKey].join(', '),
          key: `children-${key}`
        }]
      }
    });

    return result;
  }

  const beforeUpload = (file: RcFile) => {
    const extensions = ['application/vnd.ms-excel', 'test/csv'];

    if (!extensions.includes(file.type)) {
      message.error(`${file.name} has not a valid file format`);
    }
    return extensions.includes(file.type) ? true : Upload.LIST_IGNORE;
  };

  const column = useMemo(() => {
    const headerLabels = parseResult?.markedAntigenLabels || [];

    const result = headerLabels.map((label, jIndex) => {
      return {
        title: label,
        dataIndex: jIndex,
        render: (value: CrossInhibitonIndexCell, record: CrossInhibitonIndexCell[], iIndex: number) => {
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
    <div>
      <Row
        gutter={[16, 0]}
        align="middle"
      >
        <Col>
          <Typography.Text strong>Upload cross inhibition row data: </Typography.Text>
        </Col>

        <Col>
          <Upload
            accept='.csv,.xls,.xlsx'
            customRequest={handleSubmit}
            beforeUpload={beforeUpload}
            maxCount={1}
          >
            <Button icon={<UploadOutlined />}>Click to upload file</Button>
          </Upload>
        </Col>
      </Row>

      <Spin spinning={loading}>
        <Typography.Title level={4} className="table-header">Antigen by groups</Typography.Title>
        <Row>
          <Tree treeData={clusters} expandedKeys={Object.keys(clusters || [])} />
        </Row>

        <Typography.Title level={4} className="table-header">Cross inhibition data</Typography.Title>
        <Row justify="center">
          {parseResult
            ? <Table
              className="cross-inhibition-table"
              columns={column}
              dataSource={parseResult.crossInhibitionIndexes}
              pagination={false}
              scroll={{ x: 1200 }}
            />
            : <Empty />
          }
        </Row>
      </Spin>

    </div>
  );
}

export default CrossInhibition;