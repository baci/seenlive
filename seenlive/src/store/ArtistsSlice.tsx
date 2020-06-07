import { createSlice, PayloadAction, createSelector } from '@reduxjs/toolkit';
import ArtistEntry from '../entities/ArtistEntry';

export interface ArtistsState{
    nextArtistId : number;
    artistEntries : ArtistEntry[];
}

const initialState : ArtistsState = {
    nextArtistId: 0,
    artistEntries: []
};

export const ArtistsSlice = createSlice({
    name: 'ArtistsSlice',
    initialState,
    reducers: {
        AddArtistEntry (state : ArtistsState, action : PayloadAction<ArtistEntry>) {
            const existingArtistIdx: number = state.artistEntries.findIndex((entry) => entry.artist === action.payload.artist);
            if (existingArtistIdx >= 0) {
                let newArtistEntries = state.artistEntries.map((entry, entryIdx) =>
                    entryIdx === existingArtistIdx
                        ? {...entry, dateEntries: [...entry.dateEntries, action.payload.dateEntries[0]]}
                        : entry);
                return {
                    ...state,
                    artistEntries: newArtistEntries
                } as ArtistsState;
            }

            let curArtistId = state.nextArtistId;

            return {
                nextArtistId: state.nextArtistId + 1,
                artistEntries: [
                    ...state.artistEntries,
                    {
                        ...(action.payload as ArtistEntry),
                        id: curArtistId
                    },
                ]
            } as ArtistsState;
        }
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