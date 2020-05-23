import ArtistEntry from "../entities/ArtistEntry";

// Active Prompts:
export const PROMPT_NONE = "PROMPT_NONE";
export const PROMPT_ADD_ARTIST = "PROMPT_ADD_ARTIST";

// Action Names:
export const ADD_ARTIST_ENTRY = "ADD_ARTIST_ENTRY";
export const OPEN_ARTIST_ENTRY = "OPEN_ARTIST_ENTRY";
export const OPEN_ADD_ARTIST_PROMPT = "OPEN_ADD_ARTIST_PROMPT";
export const CANCEL_ADD_ARTIST_ENTRY = "CANCEL_ADD_ARTIST_ENTRY";

let nextArtistId = 0;
export const AddArtistEntry = (newArtist : ArtistEntry) => ({
    type: ADD_ARTIST_ENTRY,
    id: nextArtistId++,
    newArtist
});

export const OpenArtistEntry = (id : string) => ({
    type: OPEN_ARTIST_ENTRY,
    id
});

export const OpenAddArtistPrompt = () => ({
    type: OPEN_ADD_ARTIST_PROMPT
});

export const CancelAddArtistPrompt = () => ({
    type: CANCEL_ADD_ARTIST_ENTRY
});

// TODO:
// - DELETE_ARTIST_ENTRY
// - ADD_DATE_ENTRY
// - EDIT_DATE_ENTRY
// - DELETE_DATE_ENTRY
// - filters, search...