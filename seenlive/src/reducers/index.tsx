import { combineReducers } from 'redux';
import ArtistEntry from '../entities/ArtistEntry';
import {
    ADD_ARTIST_ENTRY,
    OPEN_ARTIST_ENTRY,
    OPEN_ADD_ARTIST_PROMPT,
    PROMPT_ADD_ARTIST,
    PROMPT_NONE,
    CANCEL_ADD_ARTIST_ENTRY,
} from '../actions/actions';

const Artists = (state: ArtistEntry[] = [], action) => {
    switch (action.type) {
        case ADD_ARTIST_ENTRY:
            const foundIdx: number = state.findIndex((entry) => entry.artist === action.newArtist.artist);
            if (foundIdx >= 0) {
                // TODO: instead of this, keep Date Entries separate with a FK to the artist entry and have a new ADD_DATE_ENTRY action
                const tmpEntries = state;
                // const tmpEntries = Object.assign({}, state);
                tmpEntries[foundIdx].dateEntries.push(action.newArtist.dateEntries[0]);
                return tmpEntries;
            }

            return [
                ...state,
                {
                    ...(action.newArtist as ArtistEntry),
                    id: action.id,
                },
            ];
    }

    return state;
};

const ExpandedArtist = (state: string = '', action) => {
    switch (action.type) {
        case OPEN_ARTIST_ENTRY:
            if (state === action.id) return '';
            return action.id;
    }

    return state;
};

const ActivePrompt = (state: string = PROMPT_NONE, action) => {
    switch (action.type) {
        case OPEN_ADD_ARTIST_PROMPT:
            return PROMPT_ADD_ARTIST;
        case ADD_ARTIST_ENTRY:
        case CANCEL_ADD_ARTIST_ENTRY:
            return PROMPT_NONE;
    }

    return state;
};

export default combineReducers({
    Artists,
    ExpandedArtist,
    ActivePrompt,
});
