import { createSlice, createSelector, createAsyncThunk } from '@reduxjs/toolkit';
import ArtistEntry from '../entities/ArtistEntry';
import { AddArtistEntry, DeleteArtistEntry, DeleteDateEntry, GetArtistEntries } from '../api/BandApi';
import ArtistCreationRequestDTO from '../entities/ArtistCreationRequestDTO';
import DateEntryDeleteRequestDTO from '../entities/DateEntryDeleteRequestDTO';
import ArtistDeleteRequestDTO from '../entities/ArtistDeleteRequestDTO';

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
    async (newEntry: ArtistCreationRequestDTO) => {
        const newArtistEntries = await AddArtistEntry(newEntry);
        return (newArtistEntries as ArtistEntry[]);
    }
);

export const DeleteArtistEntryThunk = createAsyncThunk(
    'api/Band/DeleteArtistEntry',
    async (request : ArtistDeleteRequestDTO) => {
        const result = await DeleteArtistEntry(request);
        return result;
    }
);

export const DeleteDateEntryThunk = createAsyncThunk(
    'api/Band/DeleteDateEntry',
    async (request: DateEntryDeleteRequestDTO) => {
        const result = await DeleteDateEntry(request);
        return result;
    }
);

export const GetArtistEntriesThunk = createAsyncThunk(
    'api/Band/GetArtistEntries',
    async (userId : string) => {
        const newArtistEntries = await GetArtistEntries(userId);
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
    [(state: { artists: ArtistsState }) => state.artists], // Assuming the slice of state containing artistEntries is under the 'artists' key
    (unsortedState: ArtistsState) =>
      unsortedState.artistEntries
        .map(entry => entry)
        .sort((a: ArtistEntry, b: ArtistEntry) => a.artistName.localeCompare(b.artistName))
  );