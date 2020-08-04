import { createSlice, createSelector, createAsyncThunk } from '@reduxjs/toolkit';
import ArtistEntry from '../entities/ArtistEntry';
import { AddArtistEntry, GetArtistEntries } from '../api/BandApi';
import { AxiosResponse } from 'axios';

export interface ArtistsState{
    nextArtistId : number;
    artistEntries : ArtistEntry[];
}

const initialState : ArtistsState = {
    nextArtistId: 0,
    artistEntries: []
};

export const AddArtistEntryThunk = createAsyncThunk(
    'api/Band/AddArtistEntry',
    async (newEntry: ArtistEntry) => {
        const newArtistEntries = await AddArtistEntry(newEntry);
        return (newArtistEntries as ArtistEntry[]);
    }
);

export const GetArtistEntriesThunk = createAsyncThunk(
    'api/Band/GetArtistEntries',
    async () => {
        const newArtistEntries = await GetArtistEntries();
        return (newArtistEntries as ArtistEntry[]);
    }
);

export const ArtistsSlice = createSlice({
    name: 'ArtistsSlice',
    initialState,
    reducers: {
    },
    extraReducers: builder => {
        builder.addCase(AddArtistEntryThunk.fulfilled, (state: ArtistsState, action) => {
            return {
                ...state,
                artistEntries: action.payload
            };
        })
        .addCase(GetArtistEntriesThunk.fulfilled, (state: ArtistsState, action) => {
            return {
                ...state,
                artistEntries: action.payload
            };
        });
    }
});

export const selectSortedArtists = createSelector(
    [state => state as ArtistsState],
    (unsortedState : ArtistsState) => (
        unsortedState.artistEntries.map(entry => entry).sort((a: ArtistEntry, b: ArtistEntry) => {
            return a.artist.localeCompare(b.artist);
         })
    )
);