import React, { useEffect } from 'react'
import { usePermitStore } from '../store/permitStore'
import './Dashboard.css'

const Dashboard: React.FC = () => {
  const { permits, fetchPermits, loading } = usePermitStore()

  useEffect(() => {
    fetchPermits()
  }, [])

  return (
    <div className="dashboard">
      <h2>Dashboard - Centro de Control</h2>
      
      <div className="stats-grid">
        <div className="stat-card">
          <h3>Permisos Activos</h3>
          <p className="stat-value">{permits.filter(p => p.status !== 'Closed').length}</p>
        </div>
        <div className="stat-card">
          <h3>En Revisión</h3>
          <p className="stat-value">{permits.filter(p => p.status === 'SSTReview').length}</p>
        </div>
        <div className="stat-card">
          <h3>Aprobados</h3>
          <p className="stat-value">{permits.filter(p => p.status === 'Approved').length}</p>
        </div>
        <div className="stat-card">
          <h3>Cerrados</h3>
          <p className="stat-value">{permits.filter(p => p.status === 'Closed').length}</p>
        </div>
      </div>

      <section className="recent-permits">
        <h3>Permisos Recientes</h3>
        {loading ? (
          <p>Cargando...</p>
        ) : permits.length > 0 ? (
          <table className="permits-table">
            <thead>
              <tr>
                <th>Número</th>
                <th>Descripción</th>
                <th>Estado</th>
                <th>Solicitante</th>
              </tr>
            </thead>
            <tbody>
              {permits.slice(0, 5).map(permit => (
                <tr key={permit.id}>
                  <td>{permit.number}</td>
                  <td>{permit.description}</td>
                  <td><span className={`status ${permit.status.toLowerCase()}`}>{permit.status}</span></td>
                  <td>{permit.requestedBy}</td>
                </tr>
              ))}
            </tbody>
          </table>
        ) : (
          <p>No hay permisos disponibles</p>
        )}
      </section>
    </div>
  )
}

export default Dashboard
