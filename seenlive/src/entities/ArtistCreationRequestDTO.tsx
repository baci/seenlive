import DateEntryCreationRequestDTO from './DateEntryCreationRequestDTO';

export default interface ArtistCreationRequestDTO {
    artistName: string;
    dateEntryRequests: DateEntryCreationRequestDTO[];
}
