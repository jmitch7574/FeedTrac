import './App.css'
import Logo from './LOGO.png';
import React from 'react';

function App() {

    return (
        <div>
            <a href="/Home">
                <button style={{ margin: '0 10px' }}>Home</button>
            </a>
            <a href="/Creating_modules">
                <button style={{ margin: '0 10px' }}>Creating Modules</button>
            </a>
            <a href="/Current_modules">
                <button>Current Modules</button>
            </a>
            <a href="/Tickets">
                <button style={{ margin: '0 10px' }}>Tickets</button>
            </a>
            <h1 className="Title">Staff Home</h1>
            <main className="main">
                <img src={Logo} alt="Uni of lincoln"/>

                <p style={{ color: 'black', fontSize: '30px', fontWeight: '900' }}> [Student contents] </p>

            </main>

        </div>
    );
}

export default App;
