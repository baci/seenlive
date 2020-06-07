import { combineReducers } from '@reduxjs/toolkit';
import { UISlice } from '../store/UISlice';
import { ArtistsSlice } from '../store/ArtistsSlice';

export const RootReducer = combineReducers({
    UIState: UISlice.reducer,
    ArtistsState: ArtistsSlice.reducer
});

export type RootState = ReturnType<typeof RootReducer>;