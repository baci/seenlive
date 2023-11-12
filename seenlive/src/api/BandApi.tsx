import axios from 'axios';
import ArtistCreationRequestDTO from '../entities/ArtistCreationRequestDTO';
import DateEntryDeleteRequestDTO from '../entities/DateEntryDeleteRequestDTO';
import { AuthHeader } from './AuthHeader';
import ArtistDeleteRequestDTO from '../entities/ArtistDeleteRequestDTO';

const baseUrl = 'https://localhost:5001/api/';

function GetApiConfiguration(params? : string[]){
    return {
        baseURL: baseUrl,
        transformRequest: [(data) => JSON.stringify(data)],
        headers: AuthHeader(),
        params: params
    };
}

const instance = axios.create(GetApiConfiguration());

export async function AddArtistEntry(entry : ArtistCreationRequestDTO){

    let response = await instance.post('Band/AddArtistEntry', entry, GetApiConfiguration());
    return response.data;
}

export async function DeleteArtistEntry(request : ArtistDeleteRequestDTO){

    let response = await instance.post('Band/DeleteArtistEntry', request, GetApiConfiguration());
    return response.data;
}

export async function DeleteDateEntry(request: DateEntryDeleteRequestDTO){
    let response = await instance.post('Band/DeleteDateEntry', request, GetApiConfiguration());
    return response.data;
}

export async function GetArtistEntries(userId: string) {
    let response = await instance.get('Band/GetArtistEntries', GetApiConfiguration([userId]));
    return response.data;
}
