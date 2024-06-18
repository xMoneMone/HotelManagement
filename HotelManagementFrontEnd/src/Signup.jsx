import { useNavigate, Link } from 'react-router-dom';
import React, { useContext, useEffect, useState } from "react";
import "./css/form.css"
import { UserContext } from "./App"
import useLogin from "./hooks/useLogin"
import Button from "./Button"

export default function Login() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [passwordConfirm, setPasswordConfirm] = useState('');
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [token, setToken] = useContext(UserContext)
    const [error, setError] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        document.title = "Sign up | HMT"
    })

    const handleSubmit = (e) => {
        e.preventDefault()

        const toSend = {
            colorId: 0,
            email: email,
            password: password,
            firstName: firstName,
            lastName: lastName
        }

        if (password != passwordConfirm){
            setError("Password doesn't match confirm password.")
            return;
        }
    }

    return <>
        <div className="form-container">
            <div className="form-background">
                <h1>SIGN UP</h1>
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
                    <div className="input-field">
                        <label>Confirm password:</label>
                        <input
                            type="password"
                            required
                            value={passwordConfirm}
                            onChange={(e) => setPasswordConfirm(e.target.value)}
                        />
                    </div>
                    <div className="input-field">
                        <label>First name:</label>
                        <input
                            type="text"
                            required
                            value={firstName}
                            onChange={(e) => setFirstName(e.target.value)}
                        />
                    </div>
                    <div className="input-field">
                        <label>Last name:</label>
                        <input
                            type="text"
                            required
                            value={lastName}
                            onChange={(e) => setLastName(e.target.value)}
                        />
                    </div>
                    <div className="wrong"><div className="wrong-div">{error}</div></div>
                    <div className="button-div">
                        <Button>SIGN UP</Button>
                    </div>
                </form>
                <p className='under-form-message'>Already have an account? <Link className='under-form-message-link' to="/login">Log in</Link></p>
            </div>
        </div>
    </>
}