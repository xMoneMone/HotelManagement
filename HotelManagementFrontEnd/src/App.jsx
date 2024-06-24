import React from 'react'
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import { useState } from "react"
import Home from "./Home"
import Navbar from "./Navbar"
import Signup from "./Signup"
import Login from './Login';
import Rooms from './Rooms';
import RoomDetails from './RoomDetails';
import BookingDetails from './BookingDetails';

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
              <Route exact path="/login" element={<Login/>}/>
              <Route exact path="/signup" element={<Signup/>}/>
              <Route exact path="/hotels/:pk/rooms" element={<Rooms/>}/>
              <Route exact path="/hotels/:hotelId/rooms/:roomId" element={<RoomDetails/>}/>
              <Route exact path="/hotels/:hotelId/rooms/:roomId/bookings/:bookingId" element={<BookingDetails/>}/>
            </Routes>
          </div>
        </UserContext.Provider>
      </Router>
    </>
    )
}

