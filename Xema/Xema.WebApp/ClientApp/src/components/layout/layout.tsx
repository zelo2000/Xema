import React, { FC, ReactNode } from 'react';
import { Col, Layout, Menu, Row } from 'antd';
import './layout.scss';

const { Header, Content } = Layout;

const CustomLayout: FC<{ children: ReactNode }> = ({ children }: { children: ReactNode }) => {
  return (
    <Layout className="layout">
      <Header className="header">
        <Row>
          <Col>
            <div className="logo-container">
              <span className="logo">Xema</span>
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

      {/* <Footer className="footer">YaZOKP Software ©2021 Created by Oleksandr Zelinskyi</Footer> */}
    </Layout>
  );
};

export default CustomLayout;