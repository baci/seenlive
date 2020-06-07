import { createSlice, PayloadAction, createSelector } from '@reduxjs/toolkit';
import { PROMPT_ADD_ARTIST, PROMPT_NONE } from '../actions/actions';

export interface UIState{
    ActivePrompt : string;
    ExpandedArtistID : string;
}

export const UISlice = createSlice({
    name: 'UISlice',
    initialState: {
        ActivePrompt: PROMPT_NONE,
        ExpandedArtistID: ''
    } as UIState,
    reducers: {
        OpenAddArtistPrompt (state) {
            return { ...state, ActivePrompt: PROMPT_ADD_ARTIST };
        },
        CloseAddArtistPrompt(state){
            return { ...state, ActivePrompt: PROMPT_NONE };
        },
        ToggleExpandArtistEntry (state, action : PayloadAction<string>) {
            if (state.ExpandedArtistID === action.payload) {
                return {...state, ExpandedArtistID: ''};
            } else {
                return {...state, ExpandedArtistID: action.payload};
            }
        }
    }
});

export const selectUIState = createSelector(
    [state => state as UIState],
    (oldState) => (oldState)
);