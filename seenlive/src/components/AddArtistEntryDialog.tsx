import * as React from 'react';
import 'date-fns';
import Button from '@material-ui/core/Button';
import DialogTitle from '@material-ui/core/DialogTitle';
import DialogContent from '@material-ui/core/DialogContent';
import DialogActions from '@material-ui/core/DialogActions';
import Dialog from '@material-ui/core/Dialog';
import DateFnsUtils from '@date-io/date-fns';
import {
  MuiPickersUtilsProvider,
  KeyboardDatePicker,
  DateTimePicker,
} from '@material-ui/pickers';
import { TextField } from '@material-ui/core';
import { useSelector, useDispatch } from 'react-redux';
import { ThunkDispatch } from 'redux-thunk';
import { UIState, UISlice, selectUIState } from '../store/UISlice';
import { RootState } from '../reducers/RootReducer';
import { AddArtistEntryThunk } from '../store/ArtistsSlice';
import { PROMPT_ADD_ARTIST } from '../actions/actions';
import ArtistCreationRequestDTO from '../entities/ArtistCreationRequestDTO';
import DateEntryCreationRequestDTO from '../entities/DateEntryCreationRequestDTO';
import { AnyAction } from 'redux';

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
                <MuiPickersUtilsProvider utils={DateFnsUtils}>
                    <KeyboardDatePicker
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
                </MuiPickersUtilsProvider>
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
