import DateEntryCreationRequestDTO from './DateEntryCreationRequestDTO';

export default interface ArtistCreationRequestDTO {
    userId: string;
    artistName: string;
    dateEntryRequests: DateEntryCreationRequestDTO[];
}
