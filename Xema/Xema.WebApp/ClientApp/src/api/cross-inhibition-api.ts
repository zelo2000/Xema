import instanceApi from '../utils/instance-api';
import { AxiosRequestConfig } from 'axios';
import { CrossInhibitonRawDataModel } from '../types/cross-inhibiton-raw-data-model';

export const uploadFileApi = async (formData: FormData): Promise<CrossInhibitonRawDataModel> => {
  var config: AxiosRequestConfig = { headers: { 'Content-Type': 'multipart/form-data' } };
  const result = await instanceApi.post<CrossInhibitonRawDataModel>('CrossInhibiton', formData, config);
  return result.data;
};