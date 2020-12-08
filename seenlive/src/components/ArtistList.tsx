import * as React from 'react';
import { hot } from 'react-hot-loader';
import { Typography, Box, Divider } from '@material-ui/core';
import './../assets/scss/App.scss';
import { useSelector, useDispatch } from 'react-redux';
import ArtistEntryComponent from './ArtistEntryComponent';
import { GetArtistEntriesThunk, selectSortedArtists } from '../store/ArtistsSlice';
import { UIState, UISlice, selectUIState } from '../store/UISlice';
import ArtistEntry from '../entities/ArtistEntry';
import { RootState } from '../reducers/RootReducer';
import { useEffect } from 'react';

function useArtistsSlice() {
    const dispatch = useDispatch();
    const sortedArtists : ArtistEntry[] = useSelector((state: RootState) => selectSortedArtists(state.ArtistsState));

    const getArtists = () => dispatch(GetArtistEntriesThunk());

    return {sortedArtists, getArtists};
}

function useUISlice(){
    const dispatch = useDispatch();

    const uiState : UIState = useSelector((state : RootState) => selectUIState(state.UIState));

    const toggleArtistExpanded = (artistID : string) => dispatch(UISlice.actions.ToggleExpandArtistEntry(artistID));

    return {uiState, toggleArtistExpanded};
}

function ArtistList() {

    const {sortedArtists, getArtists} = useArtistsSlice();
    const {uiState, toggleArtistExpanded} = useUISlice();

    useEffect(() => {
        getArtists();
    });

    return (
        <div className="app">
            <Box>
                <div className="entries">
                    {sortedArtists.map((e) => (
                            <ArtistEntryComponent
                                entry={e}
                                expanded={uiState.ExpandedArtistID === e.id}
                                handleChangeExpanded={toggleArtistExpanded}
                            />
                        ))}
                </div>

                <p>
                    <Divider />
                </p>
                <div className="footer">
                    <Typography variant="caption">Copyright 2020 Till Riemer.</Typography>
                </div>
            </Box>
        </div>
    );
}

declare let module: object;

export default hot(module)(ArtistList);
