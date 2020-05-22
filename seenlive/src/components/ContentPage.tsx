import * as React from "react";
import {useState} from "react";
import { hot } from "react-hot-loader";
import { Typography, Box, Divider, Button, Container, Grid, Icon } from "@material-ui/core";
import { makeStyles, Theme, createStyles } from '@material-ui/core/styles';
import AddIcon from '@material-ui/icons/Add';
import "./../assets/scss/App.scss";
import ArtistEntry from "../entities/ArtistEntry";
import ArtistEntryComponent from "./ArtistEntryComponent";
import AddArtistEntryDialog from "./AddArtistEntryDialog";
import DateEntry from "../entities/DateEntry";

export interface IProps{}

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    paper: {
      width: '80%',
      maxHeight: 435,
    },
  }),
);

function ContentPage(props : IProps)
{
    const [entries, setEntries] = useState([] as ArtistEntry[]);
    const [openAddEntry, setOpenAddEntry] = React.useState(false);
    const [expandedEntry, setExpandedEntry] = React.useState<string | boolean>(false);

    function AddArtistEntry(artist : string, date : string, location : string, remarks : string)
    {
        var dateEntry : DateEntry = {date:date, location:location, remarks:remarks};

        const foundIdx : number = entries.findIndex((entry) => entry.artist === artist);
        if(foundIdx >= 0)
        {
            const tmpEntries = entries;
            tmpEntries[foundIdx].dateEntries.push(dateEntry);
            setEntries(tmpEntries);
        }
        else
        {
            const newEntry : ArtistEntry = {artist : artist, dateEntries : [dateEntry]};
            setEntries([...entries, newEntry]);
        }        
    }

    const handleClickAddEntry = () => {
        setOpenAddEntry(true);
    };
    
    const handleCloseAddEntryDialog = (artist?: string, date?: string, location?: string, remarks?: string) => {
        setOpenAddEntry(false);

        if(artist && date)
        {
            AddArtistEntry(artist, date, location, remarks);
        }
    };

    const handleChangeEntryExpansion = (event: React.ChangeEvent<{}>, panel: string, isExpanded: boolean) => {
        setExpandedEntry(isExpanded ? panel : false);        
    };

    const classes = useStyles();

    return (
        <div className="app">
            <Box>
                <div className="buttonbar">
                    <Grid container direction="row" justify="flex-start" alignItems="center">
                        <Grid item style={{flex:1}}>
                            <Typography variant="body1">Bands seen live...</Typography>
                        </Grid>
                        <Grid item>
                            <Button variant="contained" color="secondary" endIcon={<AddIcon />} onClick={() => handleClickAddEntry()}>
                                Add Artist
                            </Button>
                            <AddArtistEntryDialog
                                classes={{
                                    paper: classes.paper,
                                }}
                                id="ringtone-menu"
                                keepMounted
                                open={openAddEntry}
                                onClose={handleCloseAddEntryDialog}
                                />
                        </Grid>
                    </Grid>
                </div>

                <p><Divider/></p>

                <div className="entries">
                {
                    entries.sort((a: ArtistEntry, b: ArtistEntry) => {
                        return a.artist.localeCompare(b.artist);
                    }).map((e, idx) => 
                        <ArtistEntryComponent entry={e} 
                            panelID={"panel-"+idx.toString()} 
                            expanded={expandedEntry === "panel-"+idx.toString()} 
                            handleChangeExpanded = {handleChangeEntryExpansion}
                        />)
                }
                </div>

                <p><Divider/></p>              
                
                <div className="footer">
                    <Typography variant="caption">Copyright 2020 Till Riemer.</Typography>
                </div>
            </Box>
        </div>
    );
}

declare let module: object;

export default hot(module)(ContentPage);
