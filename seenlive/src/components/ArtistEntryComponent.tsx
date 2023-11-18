import * as React from 'react';
import ArtistEntry from '../entities/ArtistEntry';
import {
    Typography,
    ExpansionPanel,
    ExpansionPanelSummary,
    ExpansionPanelDetails,
    Grid,
    IconButton,
} from '@material-ui/core';
import { makeStyles, Theme, createStyles } from '@material-ui/core/styles';
import { useDispatch } from 'react-redux';
import { ThunkDispatch } from 'redux-thunk';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import EditIcon from '@material-ui/icons/Edit';
import DeleteIcon from '@material-ui/icons/Delete';
import './../assets/scss/ArtistEntryComponent.scss';
import DateEntryComponent from './DateEntryComponent';
import { DeleteArtistEntryThunk, DeleteDateEntryThunk, GetArtistEntriesThunk } from '../store/ArtistsSlice';
import { RootState } from '../reducers/RootReducer';
import { AnyAction } from 'redux';

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        root: {
            width: '100%',
        },
        heading: {
            fontSize: theme.typography.pxToRem(15),
            fontWeight: 'bold',
            flexBasis: '200px',
            flexShrink: 0,
            flexGrow: 1,
        },
        secondaryHeading: {
            fontSize: theme.typography.pxToRem(15),
            color: theme.palette.text.secondary,
            flexBasis: '150px',
            flexShrink: 1,
            flexGrow: 0,
        },
        button: {
            flexBasis: '50px',
            flexShrink: 0,
            flexGrow: 0,
        },
    }),
);

function useArtistsSlice() {
    const dispatch: ThunkDispatch<RootState, void, AnyAction> = useDispatch();

    const deleteArtistEntry = (userId : string, artistEntryId : string) =>
        dispatch(DeleteArtistEntryThunk({userId, artistEntryId})).then(_ => dispatch(GetArtistEntriesThunk(userId)));
    const deleteDateEntry = (userId : string, artistId : string, dateId : string) =>
        dispatch(DeleteDateEntryThunk({userId, artistId, dateId})).then(_ => dispatch(GetArtistEntriesThunk(userId)));

    return {deleteArtistEntry, deleteDateEntry};
}

export interface ArtistEntryComponentProps {
    entry: ArtistEntry;

    expanded: boolean;
    handleChangeExpanded: (artistID: string) => void;
}

export default function ArtistEntryComponent(props: ArtistEntryComponentProps) {
    const classes = useStyles();
    const {deleteArtistEntry, deleteDateEntry} = useArtistsSlice();
    const userId = "TestUserId"; // TODO Slice nutzen?

    const timesSeen = props.entry.dateEntries.length;

    return (
        <div className="entry">
            <ExpansionPanel
                expanded={props.expanded}
                onChange={() => {
                    props.handleChangeExpanded(props.entry.id);
                }}
            >
                <ExpansionPanelSummary
                    expandIcon={<ExpandMoreIcon />}
                    aria-controls={'panel-' + props.entry.id + '-content'}
                    id={'panel-' + props.entry.id + '-header'}
                >
                    <Typography className={classes.heading}>{props.entry.artistName}</Typography>
                    <Typography className={classes.secondaryHeading}>Seen {timesSeen} times</Typography>

                    <IconButton size="small" className={classes.button} onClick={(event) => {
                        event.stopPropagation();
                        // deleteArtistEntry(props.entry.id);
                    }} onFocus={(event) => event.stopPropagation()}>
                        <EditIcon color="secondary" fontSize="small" />
                    </IconButton>

                    <IconButton size="small" className={classes.button} onClick={(event) => {
                        event.stopPropagation();
                        deleteArtistEntry(userId, props.entry.id);
                    }} onFocus={(event) => event.stopPropagation()}>
                        <DeleteIcon color="secondary" fontSize="small" />
                    </IconButton>
                </ExpansionPanelSummary>
                <ExpansionPanelDetails>
                    <Grid container direction="column" justify="center" alignItems="stretch" spacing={3} wrap={'nowrap'}>
                        {props.entry.dateEntries
                            .map((entry) => (
                                <Grid item xs={12}>
                                    <DateEntryComponent
                                        dateEntry={entry}
                                        canEdit={false}
                                        handleUserWantsToEdit={() => {
                                            /* todo(entry.id) */
                                        }}
                                        handleUserCancelsEdit={() => {
                                            /* todo(entry.id) */
                                        }}
                                        handleUserConfirmsEdit={(newEntry) => {
                                            /* todo(newEntry) */
                                        }}
                                        handleUserPressesDelete={(dateEntryId) => {
                                            deleteDateEntry(userId, props.entry.id, dateEntryId);
                                        }}
                                    />
                                </Grid>
                            ))}
                    </Grid>
                </ExpansionPanelDetails>
            </ExpansionPanel>
        </div>
    );
}
