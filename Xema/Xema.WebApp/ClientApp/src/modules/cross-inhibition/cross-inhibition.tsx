import { FC, useCallback, useState } from 'react';
import { Button, Upload, message, Typography, Empty, Row, Col } from 'antd';
import { RcFile } from 'antd/lib/upload';
import { DownloadOutlined, UploadOutlined } from '@ant-design/icons';
import { saveAs } from "file-saver";
import moment from 'moment';

import { exportXlsxApi, uploadFileApi } from '../../api/cross-inhibition-api';
import { CrossInhibitonRawDataModel } from './../../types/cross-inhibiton-raw-data-model';
import ClusterResult from './component/cluster-result/cluster-result';

import './cross-inhibition.scss';
import CrossInhibitionGrid from './component/cross-inhibition-grid/cross-inhibition-grid';

interface ICrossInhibitionProps {
  setLoading: (loading: boolean) => void
}

const CrossInhibition: FC<ICrossInhibitionProps> = ({ setLoading }: ICrossInhibitionProps) => {
  const [parseResult, setParsedResult] = useState<CrossInhibitonRawDataModel>();
  const [exportLoading, setExportLoading] = useState<boolean>(false);

  const handleSubmit = useCallback((value) => {
    setLoading(true);
    const formData = new FormData();
    formData.append('file', value.file);

    uploadFileApi(formData).then(response => {
      setParsedResult(response);
      value.onSuccess();
      setLoading(false);
    }).catch((error) => {
      value.onError(error);
      setLoading(false);
    });
  }, [setLoading]);

  const onExport = useCallback(() => {
    if (parseResult) {
      setExportLoading(true);

      exportXlsxApi(parseResult).then((response) => {
        saveAs(new Blob([response.data]), `result-${moment().format("DD-MM-YYYY-HH-mm")}.xlsx`);
        setExportLoading(false);
      }
      ).catch((error) => {
        message.error(error);
        setExportLoading(false);
      });
    }
  }, [parseResult]);

  const beforeUpload = (file: RcFile) => {
    const extensions = ['application/vnd.ms-excel', 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8', 'text/csv'];
    if (!extensions.includes(file.type)) {
      message.error(`${file.name} has not a valid file format`);
    }
    return extensions.includes(file.type) ? true : Upload.LIST_IGNORE;
  };

  return (
    <div>
      <div className="upload-container">
        <Row align="middle" justify="space-between">
          <Col>
            <div className='upload-header'>
              <Typography.Title level={5}>Cross inhibition row data: </Typography.Title>
            </div>

            <div>
              <Upload
                accept='.csv,.xls,.xlsx'
                customRequest={handleSubmit}
                beforeUpload={beforeUpload}
                maxCount={1}
              >
                <Button type="primary" icon={<UploadOutlined />}>Click to upload file</Button>
              </Upload>
            </div>
          </Col>
          <Col>
            <Button
              type="primary"
              icon={<DownloadOutlined />}
              onClick={onExport}
              loading={exportLoading}
              disabled={!parseResult}
            >
              Export XLSX
            </Button>
          </Col>
        </Row>
      </div>

      {
        parseResult
          ? <div>
            <ClusterResult data={parseResult?.clusters}>
            </ClusterResult>

            <CrossInhibitionGrid data={parseResult} />
          </div>
          : <Empty description={<Typography.Title level={4}>No uploaded file</Typography.Title>} />
      }

    </div>
  );
}

export default CrossInhibition;