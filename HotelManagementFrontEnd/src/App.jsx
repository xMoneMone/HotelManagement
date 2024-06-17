import React from 'react'
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import useUser from "./hooks/useUser"
import { useState } from "react"
import Home from "./Home"
import Navbar from "./Navbar"

export const UserContext = React.createContext()

export default function App(){
  const [token, setToken] = useState(document.cookie.replace("jwt_token=", ""))
  const {user} =  useUser(token);

  return (
    <>
      <Router>
        <UserContext.Provider value={user}>
        <Navbar/>
          <div className="page">
            <Routes>
              <Route exact path="/" element={<Home/>}/>
            </Routes>
          </div>
        </UserContext.Provider>
      </Router>
    </>
    )
}

