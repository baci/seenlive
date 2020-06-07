import * as React from 'react';
import { render } from 'react-dom';
import { Provider } from 'react-redux';
import { RootReducer } from './reducers/RootReducer';
import App from './components/App';
import { configureStore } from '@reduxjs/toolkit';

const store = configureStore({reducer: RootReducer});

render(
    <Provider store={store}>
        <App />
    </Provider>,
    document.getElementById('root'),
);
