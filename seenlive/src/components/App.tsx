import React from 'react';
import TopMenuToolbar from './TopMenuToolbar';
import AddArtistEntryDialog from './AddArtistEntryDialog';
import ArtistList from './ArtistList';
import { UISlice } from '../store/UISlice';
import { useDispatch } from 'react-redux';
import { Container, Fab, TabScrollButton, Toolbar, createStyles, makeStyles } from '@mui/material';
import KeyboardArrowUpIcon from '@mui/icons-material/KeyboardArrowUp';

const useStyles = makeStyles(() =>
    createStyles({
        paper: {
            width: '80%',
            maxHeight: 435,
        },
    }),
);

function useUISlice(){
    const dispatch = useDispatch();

    const openAddArtistPrompt = () => dispatch(UISlice.actions.OpenAddArtistPrompt());
    const setArtistFilter = (filter : string) => dispatch(UISlice.actions.SetArtistFilter(filter));

    return {openAddArtistPrompt, setArtistFilter};
}

export default function App() {
    const classes = useStyles();
    const {openAddArtistPrompt, setArtistFilter} = useUISlice();

    return (
        <div>
            <TopMenuToolbar
                handleAddArtistClicked={() => openAddArtistPrompt() }
                handleArtistFilterChange={(filter => setArtistFilter(filter))}/>

            <Toolbar id="back-to-top-anchor" />

            <AddArtistEntryDialog
                classes={{
                    paper: classes.paper,
                }}
                id="ringtone-menu"
                keepMounted
            />

            <Container>
                <ArtistList />
            </Container>

            <TabScrollButton direction={'right'} orientation={'vertical'}>
                <Fab color="secondary" size="small" aria-label="scroll back to top">
                    <KeyboardArrowUpIcon />
                </Fab>
            </TabScrollButton>
        </div>
    );
}
