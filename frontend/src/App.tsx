import React from 'react'
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import Layout from './components/Layout'
import Login from './pages/Login'
import Dashboard from './pages/Dashboard'
import Permits from './pages/Permits'
import Shifts from './pages/Shifts'
import PermitDetail from './pages/PermitDetail'
import { useAuthStore } from './store/authStore'

function App() {
  const { isAuthenticated } = useAuthStore()

  return (
    <Router>
      <Routes>
        <Route path="/login" element={<Login />} />
        {isAuthenticated ? (
          <Route element={<Layout />}>
            <Route path="/" element={<Dashboard />} />
            <Route path="/permits" element={<Permits />} />
            <Route path="/permits/:id" element={<PermitDetail />} />
            <Route path="/shifts" element={<Shifts />} />
          </Route>
        ) : (
          <Route path="*" element={<Login />} />
        )}
      </Routes>
    </Router>
  )
}

export default App
