import { useNavigate, Link } from 'react-router-dom';
import React, { useContext, useEffect, useState } from "react";
import "./css/form.css"
import { UserContext } from "./App"
import useLogin from "./hooks/useLogin"
import Button from "./Button"

export default function Login() {
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [errorMessage, setErrorMessage] = useState('')
    const [token, setToken] = useContext(UserContext)
    const {login} = useLogin()
    const navigate = useNavigate()

    useEffect(() => {
        document.title = "Login | HMT"
    })

    const handleSubmit = async (e) => {
        e.preventDefault()

        const {data, error} = await login(email, password)

        if (error){
            console.log(error)
            setErrorMessage(error)
            return;
        }
        else {
            // TODO
            // setToken(loginToken)
            console.log(data)
            setErrorMessage("")
            // document.cookie = jwt_token + "=" + loginToken;
            navigate("/")
        }
    }

    return <>
        <div className="form-container">
            <div className="form-background">
                <div className="form-title">
                    <div className="form-title-line line"></div>
                    <h1>LOG IN</h1>
                    <div className="form-title-line line"></div>
                </div>
                <form onSubmit={handleSubmit}>
                    <div className="input-field">
                        <label>Email:</label>
                        <input
                            type="text"
                            required
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                        />
                    </div>
                    <div className="input-field">
                        <label>Password:</label>
                        <input
                            type="password"
                            required
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                        />
                    </div>
                    <div className="wrong">{errorMessage}</div>
                    <div className="button-div">
                        <Button>LOG IN</Button>
                    </div>
                </form>
                <p className='under-form-message'>Don't have an account? <Link className='under-form-message-link' to="/signup">Sign up</Link></p>
            </div>
        </div>
    </>
}