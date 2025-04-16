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
                <button style={{ margin: '0 10px' }}>Current Modules</button>
            </a>
            <a href="/Tickets">
                <button style={{ margin: '0 10px' }}>Tickets</button>
            </a>
            <h1 className="Title">Current Modules</h1>

            <main className="main">
                <img src={Logo} alt="Uni of Linolcn" />

                <p style={{ color: 'black', fontSize: '30px', fontWeight: '900' }}> [Current modules] </p>
            </main>

        </div>
    );
}

export default CurrentModules;
