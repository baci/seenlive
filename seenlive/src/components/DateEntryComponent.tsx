import DateEntry from '../entities/DateEntry';
import React = require('react');
import { Typography, Container, Grid, IconButton } from '@material-ui/core';
import DeleteIcon from '@material-ui/icons/Delete';
import './../assets/scss/DateEntryComponent.scss';

export interface DateEntryComponentProps {
    dateEntry: DateEntry;

    // TODO: implement editing of date entries
    canEdit: boolean;
    handleUserWantsToEdit: () => void;
    handleUserConfirmsEdit: (newDateEntry: DateEntry) => void;
    handleUserCancelsEdit: () => void;

    handleUserPressesDelete: (dateEntryId : string) => void;
}

export default function DateEntryComponent(props: DateEntryComponentProps) {
    return (
        <Container maxWidth="xl">
            <Grid container direction="row" justify="center" alignItems="center" className="container" spacing={1}>
                <Grid item className="item" xs={2}>
                    <Typography variant="body1">{props.dateEntry.date}</Typography>
                </Grid>
                <Grid item className="item" xs={4}>
                    <Typography variant="body1">{props.dateEntry.location}</Typography>
                </Grid>
                <Grid item className="item" xs={5}>
                    <Typography variant="body1">{props.dateEntry.remarks}</Typography>
                </Grid>
                <Grid item className="item" xs={1}>
                    <IconButton onClick={_ => props.handleUserPressesDelete(props.dateEntry.id)}>
                        <DeleteIcon fontSize="small" />
                    </IconButton>
                </Grid>
            </Grid>
        </Container>
    );
}
