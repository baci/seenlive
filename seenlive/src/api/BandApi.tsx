import axios from 'axios';
import ArtistCreationRequestDTO from '../entities/ArtistCreationRequestDTO';
import DateEntryDeleteRequestDTO from '../entities/DateEntryDeleteRequestDTO';
import { AuthHeader } from './AuthHeader';

const baseUrl = 'https://localhost:5001/api/';

function GetApiConfiguration(){
    return {
        baseURL: baseUrl,
        transformRequest: [(data) => JSON.stringify(data)],
        headers: AuthHeader()
    };
}

const instance = axios.create(GetApiConfiguration());

export async function AddArtistEntry(entry : ArtistCreationRequestDTO){

    let response = await instance.post('Band/AddArtistEntry', entry, GetApiConfiguration());
    return response.data;
}

export async function DeleteArtistEntry(artistEntryId : string){

    let response = await instance.post('Band/DeleteArtistEntry', { artistEntryId }, GetApiConfiguration());
    return response.data;
}

export async function DeleteDateEntry(request: DateEntryDeleteRequestDTO){
    let response = await instance.post('Band/DeleteDateEntry', request, GetApiConfiguration());
    return response.data;
}

export async function GetArtistEntries() {
    let response = await instance.get('Band/GetArtistEntries', GetApiConfiguration());
    return response.data;
}
