import DateEntry from "../entities/DateEntry";
import React = require("react");
import { Typography, Container, Grid } from "@material-ui/core";
import "./../assets/scss/DateEntryComponent.scss";

export interface DateEntryComponentProps {
    dateEntry : DateEntry;
}

export default function DateEntryComponent(props : DateEntryComponentProps) {

    return (
        <Container maxWidth="xl">
            <Grid container direction="row" justify="center" alignItems="center" className="container" spacing={1}>
                <Grid item className="item" xs={2}>
                    <Typography variant="body1">{props.dateEntry.date}</Typography>
                </Grid>
                <Grid item className="item" xs={5}>
                    <Typography variant="body1">{props.dateEntry.location}</Typography>
                </Grid>
                <Grid item className="item" xs={5}>
                    <Typography variant="body1">{props.dateEntry.remarks}</Typography>
                </Grid>
            </Grid>
        </Container>
    );
}