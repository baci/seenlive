import axios from 'axios';
import ArtistEntry from '../entities/ArtistEntry';
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

export async function AddArtistEntry(entry : ArtistEntry){

    let response = await instance.post('Band/AddArtistEntry', entry, GetApiConfiguration());
    return response.data;
}

export async function GetArtistEntries() {
    let response = await instance.get('Band/GetArtistEntries', GetApiConfiguration());
    return response.data;
}
