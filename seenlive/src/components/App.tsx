import ContentPage from "./ContentPage";
import React = require("react");
import { Toolbar, Container, Fab } from "@material-ui/core";
import KeyboardArrowUpIcon from '@material-ui/icons/KeyboardArrowUp';

import ScrollTop from "./ScrollTop";
import TopMenuToolbar from "./TopMenuToolbar";

export interface IProps{}

export default function App(props : IProps) {

    return (
        <div>
            <TopMenuToolbar handleAddArtistClicked={() => { alert("todo") }} />

            <Toolbar id="back-to-top-anchor" />

            <Container>
                <ContentPage />
            </Container>

            <ScrollTop {...props}>
                <Fab color="secondary" size="small" aria-label="scroll back to top">
                    <KeyboardArrowUpIcon />
                </Fab>
            </ScrollTop>
            
        </div>
    );
}