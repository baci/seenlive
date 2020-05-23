import { connect } from 'react-redux';
import TopMenuToolbar from '../components/TopMenuToolbar';
import { OpenAddArtistPrompt } from '../actions/actions';

const mapStateToProps = (state, ownProps) => ({
    // empty for now
});

const mapDispatchToProps = (dispatch, ownProps) => ({
    handleAddArtistClicked: () => dispatch(OpenAddArtistPrompt())
});

export default connect(mapStateToProps, mapDispatchToProps)(TopMenuToolbar);