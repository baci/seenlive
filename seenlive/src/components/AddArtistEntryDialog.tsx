import * as React from 'react';
import 'date-fns';
import { useSelector, useDispatch } from 'react-redux';
import { ThunkDispatch } from 'redux-thunk';
import { UIState, UISlice, selectUIState } from '../store/UISlice';
import { RootState } from '../reducers/RootReducer';
import { AddArtistEntryThunk } from '../store/ArtistsSlice';
import { PROMPT_ADD_ARTIST } from '../actions/actions';
import ArtistCreationRequestDTO from '../entities/ArtistCreationRequestDTO';
import DateEntryCreationRequestDTO from '../entities/DateEntryCreationRequestDTO';
import { AnyAction } from 'redux';
import { Button, Dialog, DialogActions, DialogContent, DialogTitle, TextField } from '@mui/material';
import AdapterDateFns from '@mui/lab/AdapterDateFns';
import LocalizationProvider from '@mui/lab/LocalizationProvider';
import DatePicker from '@mui/lab/DatePicker';

export interface AddArtistEntryProps {
    classes: Record<'paper', string>;
    id: string;
    keepMounted: boolean;
}

function useUISlice(){
    const dispatch: ThunkDispatch<RootState, void, AnyAction> = useDispatch();

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
    const [date, setDate] = React.useState(null as Date | null);
    const [location, setLocation] = React.useState('');
    const [remarks, setRemarks] = React.useState('');
    const userId = "TestUserId"; // TODO Slice nutzen?


    const handleOk = () => {
        const dateString: string = (date ? date : new Date()).toLocaleDateString();

        const newEntry: ArtistCreationRequestDTO = {
            userId,
            artistName,
            dateEntryRequests: [{ date: dateString, location, remarks }] as DateEntryCreationRequestDTO[],
        };
        onConfirm(newEntry);
    };

    const handleChangeArtist = (event: React.ChangeEvent<HTMLInputElement>) => {
        setArtistName((event.target as HTMLInputElement).value);
    };

    const handleChangeDate = (date: Date | null) => {
        setDate(date ? date : new Date());
    };

    const handleChangeLocation = (event: React.ChangeEvent<HTMLInputElement>) => {
        setLocation((event.target as HTMLInputElement).value);
    };

    const handleChangeRemarks = (event: React.ChangeEvent<HTMLInputElement>) => {
        setRemarks((event.target as HTMLInputElement).value);
    };

    return (
        <Dialog
            //disableBackdropClick TODO gucken durch was das ersetzt wurde
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
                <LocalizationProvider dateAdapter={AdapterDateFns}>
                    <DatePicker
                        disableToolbar
                        disableFuture
                        required
                        autoOk
                        variant="inline"
                        inputVariant="outlined"
                        format="dd.MM.yyyy"
                        margin="normal"
                        id="concert-date"
                        label="Date"
                        value={date}
                        onChange={handleChangeDate}
                        KeyboardButtonProps={{
                            'aria-label': 'change date',
                        }}
                    />
                </LocalizationProvider>
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
