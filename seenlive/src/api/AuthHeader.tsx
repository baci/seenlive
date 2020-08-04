export function AuthHeader(){
    let token = localStorage.getItem('bearer-token');
    if (token) {
        return {'Content-Type': 'application/json', 'Authorization': 'Bearer ' + token};
    } else {
        return {'Content-Type': 'application/json'};
    }
}