import React from 'react'
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import { useState } from "react"
import Home from "./Home"
import Navbar from "./Navbar"

export const UserContext = React.createContext()

export default function App(){
  const [token, setToken] = useState(document.cookie.replace("jwt_token=", ""))

  return (
    <>
      <Router>
        <UserContext.Provider value={[token, setToken]}>
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

