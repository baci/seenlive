import * as React from "react";
import Button from '@material-ui/core/Button';
import DialogTitle from '@material-ui/core/DialogTitle';
import DialogContent from '@material-ui/core/DialogContent';
import DialogActions from '@material-ui/core/DialogActions';
import Dialog from '@material-ui/core/Dialog';
import { TextField } from "@material-ui/core";

export interface AddArtistEntryProps {
  classes: Record<'paper', string>;
  id: string;
  keepMounted: boolean;
  open: boolean;
  onClose: (artist?: string, date?: string, remarks?: string) => void;
}

export default function AddArtistEntryDialog(props: AddArtistEntryProps) {
  const { onClose, open, ...other } = props;
  const [artistName, setArtistName] = React.useState("");
  const [date, setDate] = React.useState("");
  const radioGroupRef = React.useRef<HTMLElement>(null);

  const handleEntering = () => {
    if (radioGroupRef.current != null) {
      radioGroupRef.current.focus();
    }
  };

  const handleCancel = () => {
    onClose();
  };

  const handleOk = () => {
    onClose(artistName, date, "TODO");
  };

  const handleChangeArtist = (event: React.ChangeEvent<HTMLInputElement>) => {
    setArtistName((event.target as HTMLInputElement).value);
  };

  const handleChangeDate = (event: React.ChangeEvent<HTMLInputElement>) => {
    setDate((event.target as HTMLInputElement).value);
  };

  return (
    <Dialog
      disableBackdropClick
      disableEscapeKeyDown
      maxWidth="xs"
      onEntering={handleEntering}
      aria-labelledby="confirmation-dialog-title"
      open={open}
      {...other}
    >
      <DialogTitle id="confirmation-dialog-title">Add artist entry</DialogTitle>
      <DialogContent dividers>
        <TextField required id="artist-name" label="Artist Name" variant="outlined" onChange={handleChangeArtist} />
        <p />
        <TextField required id="show-date" label="Date of Show" variant="outlined" onChange={handleChangeDate} />
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