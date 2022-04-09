import { FC, useCallback, useState } from 'react';
import { Button, Upload, message, Typography, Empty } from 'antd';
import { RcFile } from 'antd/lib/upload';
import { UploadOutlined } from '@ant-design/icons';

import { uploadFileApi } from '../../api/cross-inhibition-api';
import { CrossInhibitonRawDataModel } from './../../types/cross-inhibiton-raw-data-model';
import ClusterResult from './component/cluster-result';

import './cross-inhibition.scss';
import CrossInhibitionGrid from './component/cross-inhibition-grid/cross-inhibition-grid';

interface ICrossInhibitionProps {
  setLoading: (loading: boolean) => void
}

const CrossInhibition: FC<ICrossInhibitionProps> = ({ setLoading }: ICrossInhibitionProps) => {
  const [parseResult, setParsedResult] = useState<CrossInhibitonRawDataModel>();

  const handleSubmit = useCallback((value) => {
    setLoading(true);
    const formData = new FormData();
    formData.append('file', value.file);

    uploadFileApi(formData).then(responce => {
      setParsedResult(responce);
      value.onSuccess();
      setLoading(false);
    }).catch((error) => {
      value.onError(error);
      setLoading(false);
    });
  }, [setLoading]);

  const beforeUpload = (file: RcFile) => {
    const extensions = ['application/vnd.ms-excel', 'test/csv'];
    if (!extensions.includes(file.type)) {
      message.error(`${file.name} has not a valid file format`);
    }
    return extensions.includes(file.type) ? true : Upload.LIST_IGNORE;
  };

  return (
    <div>
      <div className="upload-container">
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