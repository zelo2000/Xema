import { notification } from 'antd';
import axios, { AxiosRequestConfig, AxiosError } from 'axios';

const instance = axios.create({
  baseURL: `${process.env.REACT_APP_API_URL}/api/`,
});

instance.interceptors.request.use((config: AxiosRequestConfig<any>) => {
  const newConfig = config;

  newConfig.headers = {
    'Content-Type': (config && config.headers && config.headers['content-type']) ? config.headers['content-type'] : 'application/json',
  };

  return newConfig;
});

instance.interceptors.response.use(
  (response) => response,
  (error: AxiosError<any>) => {
    notification.error({ message: error.message || error });
    return Promise.reject(error.response || error);
  }
);

export default instance;