import { useEffect, useState } from 'react'
import { StatCard } from '../../components/Card'
import { Table } from '../../components/Table'
import { summaryService } from '../../services/summaryService'
import type { PersonSummary, SummaryResponse } from '../../types'
import { getErrorMessage } from '../../utils/errors'
import { formatCurrency } from '../../utils/formatters'

export function Dashboard() {
  const [summary, setSummary] = useState<SummaryResponse | null>(null)
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    async function loadSummary() {
      try {
        // O dashboard e somente leitura, entao carrega o resumo atual ao entrar na pagina.
        setIsLoading(true)
        setError('')
        const data = await summaryService.getSummary()
        setSummary(data)
      } catch (err) {
        setError(getErrorMessage(err))
      } finally {
        setIsLoading(false)
      }
    }

    void loadSummary()
  }, [])

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <p className="eyebrow">Visao geral</p>
          <h1>Dashboard</h1>
        </div>
      </div>

      {isLoading && <p className="status-message">Carregando resumo...</p>}
      {error && <p className="alert alert--error">{error}</p>}

      {!isLoading && !error && summary && (
        <>
          {/* Totais gerais aparecem primeiro porque resumem toda a casa. */}
          <section className="stats-grid" aria-label="Totais gerais">
            <StatCard
              label="Receitas"
              value={summary.general.totalIncome}
              tone="income"
            />
            <StatCard
              label="Despesas"
              value={summary.general.totalExpenses}
              tone="expense"
            />
            <StatCard
              label="Saldo liquido"
              value={summary.general.netBalance}
              tone="balance"
            />
          </section>

          <section className="panel">
            <div className="panel__header">
              <h2>Totais por pessoa</h2>
            </div>
            {/* Linhas por pessoa ajudam a identificar renda, despesas e saldo de cada uma. */}
            <Table<PersonSummary>
              data={summary.people}
              emptyMessage="Nenhum total por pessoa encontrado."
              getRowKey={(person) => person.personId}
              columns={[
                {
                  key: 'personName',
                  header: 'Pessoa',
                  render: (person) => person.personName,
                },
                {
                  key: 'totalIncome',
                  header: 'Receita',
                  align: 'right',
                  render: (person) => formatCurrency(person.totalIncome),
                },
                {
                  key: 'totalExpenses',
                  header: 'Despesa',
                  align: 'right',
                  render: (person) => formatCurrency(person.totalExpenses),
                },
                {
                  key: 'balance',
                  header: 'Saldo',
                  align: 'right',
                  render: (person) => (
                    <span className={person.balance < 0 ? 'text-danger' : 'text-income'}>
                      {formatCurrency(person.balance)}
                    </span>
                  ),
                },
              ]}
            />
          </section>
        </>
      )}
    </div>
  )
}
