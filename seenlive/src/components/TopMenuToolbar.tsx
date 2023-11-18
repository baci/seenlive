import MenuIcon from '@material-ui/icons/Menu';
import SearchIcon from '@material-ui/icons/Search';
import AddIcon from '@material-ui/icons/Add';
import {
    AppBar,
    Toolbar,
    IconButton,
    Typography,
    InputBase,
    makeStyles,
    Theme,
    createStyles,
    fade,
    Button,
} from '@material-ui/core';
import React from 'react';

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        root: {
            flexGrow: 1,
            margin: 0.2,
        },
        menuButton: {
            marginRight: theme.spacing(2),
        },
        title: {
            flexGrow: 1,
            display: 'none',
            [theme.breakpoints.up('sm')]: {
                display: 'block',
            },
        },
        search: {
            position: 'relative',
            borderRadius: theme.shape.borderRadius,
            backgroundColor: fade(theme.palette.common.white, 0.15),
            '&:hover': {
                backgroundColor: fade(theme.palette.common.white, 0.25),
            },
            marginRight: theme.spacing(2),
            marginLeft: theme.spacing(1),
            width: 'auto',
        },
        searchIcon: {
            padding: theme.spacing(0, 2),
            height: '100%',
            position: 'absolute',
            pointerEvents: 'none',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
        },
        addArtistButton: {
            // TODO fixme
            // displayInside: 'none',
            // [theme.breakpoints.up('sm')]: {
            //    displayInside: 'block',
            // },
        },
        addArtistLabel: {
            // display: 'none',
            // [theme.breakpoints.up('sm')]: {
            //    display: 'block',
            // },
        },
        inputRoot: {
            color: 'inherit',
        },
        inputInput: {
            padding: theme.spacing(1, 1, 1, 0),
            // vertical padding + font size from searchIcon
            paddingLeft: `calc(1em + ${theme.spacing(4)}px)`,
            transition: theme.transitions.create('width'),
            width: '100%',
            [theme.breakpoints.up('sm')]: {
                width: '12ch',
                '&:focus': {
                    width: '20ch',
                },
            },
        },
    }),
);

export interface IProps {
    handleAddArtistClicked: () => void;
    handleArtistFilterChange: (filter : string) => void;
}

export default function TopMenuToolbar(props: IProps) {
    const classes = useStyles();

    return (
        <AppBar>
            <Toolbar className={classes.root}>
                <IconButton edge="start" className={classes.menuButton} color="inherit" aria-label="open drawer">
                    <MenuIcon />
                </IconButton>

                <Typography variant="h6" className={classes.title}>
                    Seen Live
                </Typography>

                <div className={classes.search}>
                    <div className={classes.searchIcon}>
                        <SearchIcon />
                    </div>
                    <InputBase
                        placeholder="Search artists…"
                        classes={{
                            root: classes.inputRoot,
                            input: classes.inputInput,
                        }}
                        inputProps={{ 'aria-label': 'search' }}
                        onChange={(e) => props.handleArtistFilterChange(e.target.value)}
                    />
                </div>

                <Button variant="contained" color="secondary" endIcon={<AddIcon />} className={classes.addArtistButton}>
                    <Typography
                        className={classes.addArtistLabel}
                        variant="button"
                        onClick={() => {
                            props.handleAddArtistClicked();
                        }}
                    >
                        Add Artist
                    </Typography>
                </Button>
            </Toolbar>
        </AppBar>
    );
}
