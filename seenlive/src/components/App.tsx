import React = require('react');
import { Toolbar, Container, Fab } from '@material-ui/core';
import { makeStyles, Theme, createStyles } from '@material-ui/core/styles';
import KeyboardArrowUpIcon from '@material-ui/icons/KeyboardArrowUp';
import ScrollTop from './ScrollTop';
import VisibleArtistList from '../containers/VisibleArtistList';
import TopMenuToolbarContainer from '../containers/TopMenuToolbarContainer';
import AddArtistEntryContainer from '../containers/AddArtistEntryContainer';

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        paper: {
            width: '80%',
            maxHeight: 435,
        },
    }),
);

export interface AppProps {}

export default function App(props: AppProps) {
    const classes = useStyles();

    return (
        <div>
            <TopMenuToolbarContainer />

            <Toolbar id="back-to-top-anchor" />

            <AddArtistEntryContainer
                classes={{
                    paper: classes.paper,
                }}
                id="ringtone-menu"
                keepMounted
            />

            <Container>
                <VisibleArtistList />
            </Container>

            <ScrollTop {...props}>
                <Fab color="secondary" size="small" aria-label="scroll back to top">
                    <KeyboardArrowUpIcon />
                </Fab>
            </ScrollTop>
        </div>
    );
}
