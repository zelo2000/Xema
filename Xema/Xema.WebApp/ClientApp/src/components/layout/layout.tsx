import React, { FC, ReactNode } from 'react';
import { Col, Layout, Menu, Row } from 'antd';
import './layout.scss';

const { Header, Content } = Layout;

const CustomLayout: FC<{ children: ReactNode }> = ({ children }: { children: ReactNode }) => {
  return (
    <Layout className="layout">
      <Header className="header">
        <Row align="middle">
          <Col>
            <div className="logo-container">
              <img className="logo" src="https://xema.in.ua/wp-content/themes/xema-2.7/dist/assets/images/theme/xemaLogo.svg" alt=""></img>
              <img className="logo-title" src="https://xema.in.ua/wp-content/themes/xema-2.7/dist/assets/images/theme/xemaLogoText.svg" alt="Xema Labs"></img>
            </div>
          </Col>
          <Col>
            <Menu theme="dark" mode="horizontal" defaultSelectedKeys={['home']}>
              <Menu.Item key='home'>Home</Menu.Item>
            </Menu>
          </Col>
        </Row>
      </Header>

      <Content>
        <div className="site-layout-content">{children}</div>
      </Content>
    </Layout>
  );
};

export default CustomLayout;