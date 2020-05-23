import * as React from "react";
import { hot } from "react-hot-loader";
import { Typography, Box, Divider } from "@material-ui/core";
import "./../assets/scss/App.scss";
import ArtistEntry from "../entities/ArtistEntry";
import ArtistEntryContainer from "../containers/ArtistEntryContainer";

export interface ArtistListProps{
    entries : ArtistEntry[];
}

function ArtistList(props : ArtistListProps)
{
    return (
        <div className="app">
            <Box>  
                <div className="entries">
                {
                    props.entries.sort((a: ArtistEntry, b: ArtistEntry) => {
                        return a.artist.localeCompare(b.artist);
                    }).map((e) => <ArtistEntryContainer entry={e} />)
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

export default hot(module)(ArtistList);
