import DateEntry from './DateEntry';

export default interface ArtistEntry {
    id: string;
    artist: string;
    dateEntries: DateEntry[];
}
