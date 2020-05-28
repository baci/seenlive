import * as React from 'react';
import Button from '@material-ui/core/Button';
import DialogTitle from '@material-ui/core/DialogTitle';
import DialogContent from '@material-ui/core/DialogContent';
import DialogActions from '@material-ui/core/DialogActions';
import Dialog from '@material-ui/core/Dialog';
import { TextField } from '@material-ui/core';
import ArtistEntry from '../entities/ArtistEntry';

export interface AddArtistEntryProps {
    classes: Record<'paper', string>;
    id: string;
    keepMounted: boolean;
    open: boolean;
    onConfirm: (newEntry: ArtistEntry) => void; // TODO revert to injecting raw data here
    onCancel: () => void;
}

export default function AddArtistEntryDialog(props: AddArtistEntryProps) {
    const { onConfirm, onCancel, open, ...other } = props;

    const [artistName, setArtistName] = React.useState('');
    const [date, setDate] = React.useState('');
    const [location, setLocation] = React.useState('');
    const [remarks, setRemarks] = React.useState('');

    const handleCancel = () => {
        onCancel();
    };

    const handleOk = () => {
        const newEntry: ArtistEntry = {
            id: '',
            artist: artistName,
            dateEntries: [{ id: '', date: date, location: location, remarks: remarks }],
        };
        onConfirm(newEntry);
    };

    const handleChangeArtist = (event: React.ChangeEvent<HTMLInputElement>) => {
        setArtistName((event.target as HTMLInputElement).value);
    };

    const handleChangeDate = (event: React.ChangeEvent<HTMLInputElement>) => {
        setDate((event.target as HTMLInputElement).value);
    };

    const handleChangeLocation = (event: React.ChangeEvent<HTMLInputElement>) => {
        setLocation((event.target as HTMLInputElement).value);
    };

    const handleChangeRemarks = (event: React.ChangeEvent<HTMLInputElement>) => {
        setRemarks((event.target as HTMLInputElement).value);
    };

    return (
        <Dialog
            disableBackdropClick
            disableEscapeKeyDown
            maxWidth="xs"
            aria-labelledby="confirmation-dialog-title"
            open={open}
            {...other}
        >
            <DialogTitle id="confirmation-dialog-title">Add artist entry</DialogTitle>
            <DialogContent dividers>
                <TextField
                    required
                    id="artist-name"
                    label="Artist Name"
                    variant="outlined"
                    onChange={handleChangeArtist}
                />
                <p />
                <TextField
                    required
                    id="show-date"
                    label="Date of Show"
                    variant="outlined"
                    onChange={handleChangeDate}
                />
                <p />
                <TextField id="show-location" label="Location" variant="outlined" onChange={handleChangeLocation} />
                <p />
                <TextField id="show-remarks" label="Remarks" variant="outlined" onChange={handleChangeRemarks} />
            </DialogContent>
            <DialogActions>
                <Button autoFocus onClick={handleCancel} color="primary">
                    Cancel
                </Button>
                <Button onClick={handleOk} color="primary">
                    Ok
                </Button>
            </DialogActions>
        </Dialog>
    );
}
