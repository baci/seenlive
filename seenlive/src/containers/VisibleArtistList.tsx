import { connect } from 'react-redux';
import ArtistList from '../components/ArtistList';

const mapStateToProps = (state) => ({
    entries: state.Artists,
});

const mapDispatchToProps = (dispatch) => ({
    // empty for now
});

export default connect(mapStateToProps, mapDispatchToProps)(ArtistList);
