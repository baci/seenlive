import ContentPage from "./ContentPage";
import React = require("react");
import { AppBar, Toolbar, Typography, Container, Fab } from "@material-ui/core";
import KeyboardArrowUpIcon from '@material-ui/icons/KeyboardArrowUp';
import ScrollTop from "./ScrollTop";

export interface IProps{}

export default function MainView(props : IProps) {

    return (
        <div>
            <AppBar>
                <Toolbar>
                    <Typography variant="h6">Seen Live</Typography>
                </Toolbar>
            </AppBar>
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