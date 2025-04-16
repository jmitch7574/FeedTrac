import './App.css'
import React from 'react';
import Logo from './LOGO.png';

function CurrentModules() {
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
            <h1 className="Title">Tickets</h1>

            <main className="main">
                <img src={Logo} alt="Uni of Lincoln" />

                <p style={{ color: 'black', fontSize: '30px', fontWeight: '900' }}> [Tickets] </p>
            </main>

        </div>
    );
}

export default CurrentModules;
