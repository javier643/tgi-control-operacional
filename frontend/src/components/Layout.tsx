import React from 'react'
import { Outlet, useNavigate } from 'react-router-dom'
import { useAuthStore } from '../store/authStore'
import './Layout.css'

const Layout: React.FC = () => {
  const { user, logout } = useAuthStore()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  return (
    <div className="layout">
      <header className="header">
        <div className="header-content">
          <h1>TGI Control Operacional</h1>
          <nav className="nav">
            <a href="/" className="nav-link">Dashboard</a>
            <a href="/permits" className="nav-link">Permisos</a>
            <a href="/shifts" className="nav-link">Turnos</a>
          </nav>
          <div className="user-menu">
            <span>{user?.username} ({user?.role})</span>
            <button onClick={handleLogout} className="logout-btn">Salir</button>
          </div>
        </div>
      </header>
      <main className="main">
        <Outlet />
      </main>
      <footer className="footer">
        <p>&copy; 2026 TGI Control Operacional - Todos los derechos reservados</p>
      </footer>
    </div>
  )
}

export default Layout
