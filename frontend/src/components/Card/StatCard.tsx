import { formatCurrency } from '../../utils/formatters'
import { Card } from './Card'

interface StatCardProps {
  label: string
  value: number
  tone?: 'income' | 'expense' | 'balance'
}

export function StatCard({ label, value, tone = 'balance' }: StatCardProps) {
  return (
    <Card className={`stat-card stat-card--${tone}`}>
      <span>{label}</span>
      <strong>{formatCurrency(value)}</strong>
    </Card>
  )
}
