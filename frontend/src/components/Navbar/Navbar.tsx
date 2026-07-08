import { NavLink } from 'react-router-dom'

const links = [
  { to: '/', label: 'Dashboard' },
  { to: '/people', label: 'Pessoas' },
  { to: '/transactions', label: 'Transacoes' },
]

export function Navbar() {
  return (
    <header className="navbar">
      <div className="navbar__inner">
        <span className="navbar__brand">Controle Residencial</span>
        <nav className="navbar__links" aria-label="Principal">
          {links.map((link) => (
            <NavLink
              key={link.to}
              to={link.to}
              className={({ isActive }) =>
                isActive ? 'navbar__link navbar__link--active' : 'navbar__link'
              }
              end={link.to === '/'}
            >
              {link.label}
            </NavLink>
          ))}
        </nav>
      </div>
    </header>
  )
}
