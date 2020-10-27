import DateEntry from './DateEntry';

export default interface ArtistEntry {
    id: string;
    artistName: string;
    dateEntries: DateEntry[];
}
