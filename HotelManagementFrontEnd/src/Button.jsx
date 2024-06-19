import './css/button.css'

export default function Button({children, onClick = () => {}}) {
    return <button className="button" onClick={onClick}>{children}</button>
}