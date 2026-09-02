import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuthStore } from '../store/authStore'
import './Login.css'

const Login: React.FC = () => {
  const [username, setUsername] = useState('')
  const [role, setRole] = useState('Operator')
  const [company, setCompany] = useState('TGI Demo')
  const [center, setCenter] = useState('Centro Principal')
  const [shift, setShift] = useState('Diurno')
  const navigate = useNavigate()
  const { login } = useAuthStore()

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    try {
      await login(username, '', role)
      navigate('/')
    } catch (error) {
      console.error('Login failed:', error)
    }
  }

  return (
    <div className="login-container">
      <div className="login-card">
        <h1>Centro de Control Operacional TGI</h1>
        <p className="subtitle">MVP - Modo Demostración</p>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="username">Nombre de Usuario</label>
            <input
              id="username"
              type="text"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              placeholder="Ingrese su nombre de usuario"
              required
            />
          </div>

          <div className="form-group">
            <label htmlFor="role">Rol</label>
            <select
              id="role"
              value={role}
              onChange={(e) => setRole(e.target.value)}
            >
              <option value="Admin">Administrador</option>
              <option value="Supervisor">Supervisor</option>
              <option value="SSTReviewer">Revisor SST</option>
              <option value="AreaValidator">Validador de Área</option>
              <option value="Operator">Operador</option>
              <option value="Viewer">Visualizador</option>
            </select>
          </div>

          <div className="form-group">
            <label htmlFor="company">Empresa</label>
            <input
              id="company"
              type="text"
              value={company}
              onChange={(e) => setCompany(e.target.value)}
              placeholder="Empresa"
            />
          </div>

          <div className="form-group">
            <label htmlFor="center">Centro Operacional</label>
            <input
              id="center"
              type="text"
              value={center}
              onChange={(e) => setCenter(e.target.value)}
              placeholder="Centro Operacional"
            />
          </div>

          <div className="form-group">
            <label htmlFor="shift">Turno</label>
            <select
              id="shift"
              value={shift}
              onChange={(e) => setShift(e.target.value)}
            >
              <option value="Diurno">Diurno (6:00 - 14:00)</option>
              <option value="Vespertino">Vespertino (14:00 - 22:00)</option>
              <option value="Nocturno">Nocturno (22:00 - 6:00)</option>
            </select>
          </div>

          <button type="submit" className="login-btn">Acceder al Sistema</button>
        </form>
        <p className="info">No requiere contraseña en modo demostración</p>
      </div>
    </div>
  )
}

export default Login
