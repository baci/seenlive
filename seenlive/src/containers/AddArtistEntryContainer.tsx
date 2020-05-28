import { connect } from 'react-redux';
import AddArtistEntryDialog from '../components/AddArtistEntryDialog';
import { AddArtistEntry, PROMPT_ADD_ARTIST, CancelAddArtistPrompt } from '../actions/actions';
import ArtistEntry from '../entities/ArtistEntry';

const mapStateToProps = (state, ownProps) => ({
    ...ownProps,
    open: state.ActivePrompt === PROMPT_ADD_ARTIST,
});

const mapDispatchToProps = (dispatch) => ({
    onConfirm: (newEntry: ArtistEntry) => dispatch(AddArtistEntry(newEntry)),
    onCancel: () => dispatch(CancelAddArtistPrompt()),
});

export default connect(mapStateToProps, mapDispatchToProps)(AddArtistEntryDialog);
