import { connect } from 'react-redux';
import ArtistEntryComponent from '../components/ArtistEntryComponent';
import { OpenArtistEntry } from '../actions/actions';

const mapStateToProps = (state, ownProps) => ({
    ...ownProps,
    timesSeen: ownProps.entry.dateEntries.length,
    expanded: (state.ExpandedArtist === ownProps.entry.id)
});

const mapDispatchToProps = (dispatch) => ({
    handleChangeExpanded : (artistID: string) => dispatch(OpenArtistEntry(artistID)) 
});

export default connect(mapStateToProps, mapDispatchToProps)(ArtistEntryComponent);