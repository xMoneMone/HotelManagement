import Button from "./Button"
import { useContext, useEffect } from "react"
import { UserContext } from "./App"
import { Link } from 'react-router-dom';
import './css/home.css'

function Home () {
    const [token, setToken] = useContext(UserContext)

    useEffect(() => {
        document.title = 'Home | HMT';
      }, []);
      
    return  <>
                {token && <div className="home-not-authorized">
                            
                            <Link to=""><Button>SIGN UP</Button></Link>
                            <Link to=""><Button>LOG IN</Button> </Link>
                            
                </div>} 
            </>
}

export default Home