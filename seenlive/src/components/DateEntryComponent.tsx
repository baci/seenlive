import DateEntry from '../entities/DateEntry';
import React from 'react';
import './../assets/scss/DateEntryComponent.scss';
import { Container, Grid, IconButton, Typography } from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';

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
            <Grid container direction="row" justifyContent="center" alignItems="center" className="container" spacing={1}>
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
