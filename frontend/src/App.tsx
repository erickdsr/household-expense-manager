import { BrowserRouter, Route, Routes } from 'react-router-dom'
import { Layout } from './components/Layout'
import { Dashboard } from './pages/Dashboard'
import { People } from './pages/People'
import { Transactions } from './pages/Transactions'

function App() {
  return (
    <BrowserRouter>
      <Layout>
        {/* Rotas principais renderizadas dentro do layout compartilhado com navegacao. */}
        <Routes>
          <Route path="/" element={<Dashboard />} />
          <Route path="/people" element={<People />} />
          <Route path="/transactions" element={<Transactions />} />
        </Routes>
      </Layout>
    </BrowserRouter>
  )
}

export default App
