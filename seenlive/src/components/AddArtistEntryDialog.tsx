import * as React from 'react';
import Button from '@material-ui/core/Button';
import DialogTitle from '@material-ui/core/DialogTitle';
import DialogContent from '@material-ui/core/DialogContent';
import DialogActions from '@material-ui/core/DialogActions';
import Dialog from '@material-ui/core/Dialog';
import { TextField } from '@material-ui/core';
import { useSelector, useDispatch } from 'react-redux';
import { UIState, UISlice, selectUIState } from '../store/UISlice';
import { RootState } from '../reducers/RootReducer';
import { AddArtistEntryThunk } from '../store/ArtistsSlice';
import { PROMPT_ADD_ARTIST } from '../actions/actions';
import ArtistCreationRequestDTO from '../entities/ArtistCreationRequestDTO';
import DateEntryCreationRequestDTO from '../entities/DateEntryCreationRequestDTO';

export interface AddArtistEntryProps {
    classes: Record<'paper', string>;
    id: string;
    keepMounted: boolean;
}

function useUISlice(){
    const dispatch = useDispatch();

    const uiState : UIState = useSelector((state: RootState) => selectUIState(state.UIState));

    const closePrompt = () => dispatch(UISlice.actions.CloseAddArtistPrompt());
    const addArtist = (newEntry : ArtistCreationRequestDTO) => dispatch(AddArtistEntryThunk(newEntry));

    const onConfirm = (newEntry : ArtistCreationRequestDTO) => {
        closePrompt();
        addArtist(newEntry);
    };
    const onCancel = closePrompt;

    return {uiState, onConfirm, onCancel};
}

export default function AddArtistEntryDialog(props: AddArtistEntryProps) {
    const { uiState, onConfirm, onCancel } = useUISlice();
    const { ...other } = props;

    const [artistName, setArtistName] = React.useState('');
    const [date, setDate] = React.useState('');
    const [location, setLocation] = React.useState('');
    const [remarks, setRemarks] = React.useState('');

    const handleOk = () => {
        const newEntry: ArtistCreationRequestDTO = {
            artistName,
            dateEntryRequests: [{ date, location, remarks }] as DateEntryCreationRequestDTO[],
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
            open={uiState.ActivePrompt === PROMPT_ADD_ARTIST}
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
                <Button autoFocus onClick={onCancel} color="primary">
                    Cancel
                </Button>
                <Button onClick={handleOk} color="primary">
                    Ok
                </Button>
            </DialogActions>
        </Dialog>
    );
}
