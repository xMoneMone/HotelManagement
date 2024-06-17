import Button from "./Button"
import { useContext, useEffect } from "react"
import { UserContext } from "./App"
import { Link } from 'react-router-dom';

function Home () {

    useEffect(() => {
        document.title = 'Home | HMT';
      }, []);
      
    return  <>
                            
                            <Link to=""><Button> SIGN UP</Button></Link>
                            <Link to=""><Button>LOG IN</Button> </Link>
                            
                </div>} 
            </>
}

export default Home