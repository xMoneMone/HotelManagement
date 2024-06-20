import { axios_base } from "../util/axios";

export default () => {
    const login = async (email, password) =>
    {
        try
        {
            const {data} = await axios_base.post("users/login", {
                email: email,
                password: password
            })
            return {data};
        }
        catch (error) {
            if (error.response) {
                return {error: error.response.data}
            }
        };

    }

    return {login}
}
