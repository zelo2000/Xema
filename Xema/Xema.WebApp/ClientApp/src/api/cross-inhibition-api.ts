import instanceApi from '../utils/instance-api';
import { AxiosRequestConfig, AxiosResponse } from 'axios';
import { CrossInhibitonRawDataModel } from '../types/cross-inhibiton-raw-data-model';

export const uploadFileApi = async (formData: FormData): Promise<CrossInhibitonRawDataModel> => {
  var config: AxiosRequestConfig = { headers: { 'Content-Type': 'multipart/form-data' } };
  const result = await instanceApi.post<CrossInhibitonRawDataModel>('CrossInhibiton/upload', formData, config);
  return result.data;
};

export const exportXlsxApi = async (values: CrossInhibitonRawDataModel): Promise<AxiosResponse<Blob>> => {
  return await instanceApi.post<Blob>('CrossInhibiton/export-xlsx', values, { responseType: 'blob' });
};