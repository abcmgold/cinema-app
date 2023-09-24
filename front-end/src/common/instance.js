import axios from 'axios'
// import { MISAResource } from './resource';
// import ENUM from './enum';
const instance = axios.create({baseURL: 'https://localhost:7102/api/'});

const jwtToken = sessionStorage.getItem("jwtToken");

instance.defaults.headers.common['Authorization'] = `Bearer ${jwtToken}`;

// instance.interceptors.response.use((response) => {
//     return response
// }, (error) => {
//     if (!error.response) {
//         return Promise.reject({
//             message: MISAResource['vn-VI'].serverNotResponse,
//             statusCode: ENUM.statusCode.serverError
//         })
//     } else {
//         if (error.response.data.Errors) {
//             let listError = [];
//             error.response.data.Errors.forEach((err) => {
//                 listError.push(err.ErrorMessage);
//             })
//             return Promise.reject({message: listError, statusCode: error.response.status});
//         } else {
//             return Promise.reject({message: error.response.data.UserMessage, statusCode: error.response.status});
//         }
//     }

// })

export default instance;
