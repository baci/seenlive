import * as React from "react";
import ArtistEntry from "../entities/ArtistEntry";
import { Typography, ExpansionPanel, ExpansionPanelSummary, ExpansionPanelDetails, IconButton, Grid } from "@material-ui/core";
import { makeStyles, Theme, createStyles } from '@material-ui/core/styles';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import EditIcon from '@material-ui/icons/Edit';
import "./../assets/scss/ArtistEntryComponent.scss";
import DateEntryComponent from "./DateEntryComponent";

export interface ArtistEntryComponentProps {
    entry : ArtistEntry;

    panelID : string;
    expanded : boolean;
    handleChangeExpanded : (event: React.ChangeEvent<{}>, panel: string, isExpanded: boolean) => void;
}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      width: '100%',
    },
    heading: {
      fontSize: theme.typography.pxToRem(15),
      fontWeight: "bold",
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
  }),
);

export default function ArtistEntryComponent(props : ArtistEntryComponentProps) {
    const classes = useStyles();

    return(
        <div className="entry">
            <ExpansionPanel expanded={props.expanded} onChange={(event) => {props.handleChangeExpanded(event, props.panelID, !props.expanded)}}>
                <ExpansionPanelSummary
                  expandIcon={<ExpandMoreIcon />}
                  aria-controls={"panel-"+props.panelID+"-content"}
                  id={"panel-"+props.panelID+"-header"}
                >
                <Typography className={classes.heading}>{props.entry.artist}</Typography>
                <Typography className={classes.secondaryHeading}>Seen {props.entry.dateEntries.length} times</Typography>
                
                <IconButton><EditIcon color="secondary" /></IconButton>
                
                </ExpansionPanelSummary>
                <ExpansionPanelDetails>
                  <Grid container direction="column" justify="center" alignItems="stretch" spacing={3}>
                  {
                    props.entry.dateEntries
                      .sort((a,b)=> { return a.date.localeCompare(b.date) })
                      .map((entry) =>
                        <Grid item xs={12}>
                          <DateEntryComponent dateEntry={entry} />
                          </Grid>
                    )
                  }
                  </Grid>
                </ExpansionPanelDetails>
            </ExpansionPanel>
        </div>
    );
}