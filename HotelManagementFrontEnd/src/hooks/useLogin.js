import { useState } from "react";
import { axios_base } from "../util/axios";

export default async (email, password) => {
    const [token, setToken] = useState("");
    const [error, setError] = useState("");

    const {data} = await axios_base.get("users/login", {headers: {authorization: token},
            data: 
        {
            email: email,
            password: password
        }
    })
    .catch(function (error) {
        if (error.response) {
            setError(error.response);
            console.log(error.response.data);
            console.log(error.response.status);
            console.log(error.response.headers);
        }
    });
    console.log(data)
    setToken(data)

    return {token, error}
}
