import { useEffect, useState } from 'react'
import type { FormEvent } from 'react'
import { Button } from '../../components/Button'
import { Input } from '../../components/Input'
import { Select } from '../../components/Select'
import { Table } from '../../components/Table'
import { personService } from '../../services/personService'
import { transactionService } from '../../services/transactionService'
import type { FinancialTransaction, Person, TransactionType } from '../../types'
import { getErrorMessage } from '../../utils/errors'
import { formatCurrency, formatDate } from '../../utils/formatters'

export function Transactions() {
  const [transactions, setTransactions] = useState<FinancialTransaction[]>([])
  const [people, setPeople] = useState<Person[]>([])
  const [description, setDescription] = useState('')
  const [amount, setAmount] = useState('')
  const [type, setType] = useState<TransactionType>('Expense')
  const [personId, setPersonId] = useState('')
  const [isLoading, setIsLoading] = useState(true)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [error, setError] = useState('')
  const [success, setSuccess] = useState('')

  async function loadPageData() {
    try {
      setIsLoading(true)
      setError('')
      const [transactionsData, peopleData] = await Promise.all([
        transactionService.getTransactions(),
        personService.getPeople(),
      ])
      setTransactions(transactionsData)
      setPeople(peopleData)
    } catch (err) {
      setError(getErrorMessage(err))
    } finally {
      setIsLoading(false)
    }
  }

  useEffect(() => {
    async function loadInitialPageData() {
      try {
        setIsLoading(true)
        setError('')
        const [transactionsData, peopleData] = await Promise.all([
          transactionService.getTransactions(),
          personService.getPeople(),
        ])
        setTransactions(transactionsData)
        setPeople(peopleData)
      } catch (err) {
        setError(getErrorMessage(err))
      } finally {
        setIsLoading(false)
      }
    }

    void loadInitialPageData()
  }, [])

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    setError('')
    setSuccess('')

    const trimmedDescription = description.trim()
    const parsedAmount = Number(amount)
    const parsedPersonId = Number(personId)

    if (!trimmedDescription) {
      setError('Descricao e obrigatoria.')
      return
    }

    if (amount === '' || Number.isNaN(parsedAmount) || parsedAmount <= 0) {
      setError('Valor deve ser maior que zero.')
      return
    }

    if (!personId || Number.isNaN(parsedPersonId)) {
      setError('Selecione uma pessoa cadastrada.')
      return
    }

    try {
      setIsSubmitting(true)
      await transactionService.createTransaction({
        description: trimmedDescription,
        amount: parsedAmount,
        type,
        personId: parsedPersonId,
      })
      setDescription('')
      setAmount('')
      setType('Expense')
      setPersonId('')
      setSuccess('Transacao cadastrada com sucesso.')
      await loadPageData()
    } catch (err) {
      setError(getErrorMessage(err))
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <p className="eyebrow">Movimentacoes</p>
          <h1>Transacoes</h1>
        </div>
      </div>

      {error && <p className="alert alert--error">{error}</p>}
      {success && <p className="alert alert--success">{success}</p>}

      <section className="panel">
        <div className="panel__header">
          <h2>Nova transacao</h2>
        </div>
        <form className="form-grid form-grid--wide" onSubmit={handleSubmit}>
          <Input
            label="Descricao"
            name="description"
            type="text"
            value={description}
            onChange={(event) => setDescription(event.target.value)}
            required
          />
          <Input
            label="Valor"
            name="amount"
            type="number"
            min="0.01"
            step="0.01"
            value={amount}
            onChange={(event) => setAmount(event.target.value)}
            required
          />
          <Select
            label="Tipo"
            name="type"
            value={type}
            onChange={(event) => setType(event.target.value as TransactionType)}
          >
            <option value="Expense">Expense</option>
            <option value="Income">Income</option>
          </Select>
          <Select
            label="Pessoa"
            name="personId"
            value={personId}
            onChange={(event) => setPersonId(event.target.value)}
            required
          >
            <option value="">Selecione</option>
            {people.map((person) => (
              <option key={person.id} value={person.id}>
                {person.name}
              </option>
            ))}
          </Select>
          <div className="form-actions">
            <Button type="submit" disabled={isSubmitting || people.length === 0}>
              {isSubmitting ? 'Salvando...' : 'Cadastrar'}
            </Button>
          </div>
        </form>
      </section>

      <section className="panel">
        <div className="panel__header">
          <h2>Transacoes cadastradas</h2>
        </div>
        {isLoading ? (
          <p className="status-message">Carregando transacoes...</p>
        ) : (
          <Table<FinancialTransaction>
            data={transactions}
            emptyMessage="Nenhuma transacao cadastrada."
            getRowKey={(transaction) => transaction.id}
            columns={[
              {
                key: 'id',
                header: 'ID',
                render: (transaction) => transaction.id,
              },
              {
                key: 'description',
                header: 'Descricao',
                render: (transaction) => transaction.description,
              },
              {
                key: 'amount',
                header: 'Valor',
                align: 'right',
                render: (transaction) => formatCurrency(transaction.amount),
              },
              {
                key: 'type',
                header: 'Tipo',
                render: (transaction) => (
                  <span
                    className={
                      transaction.type === 'Income'
                        ? 'badge badge--income'
                        : 'badge badge--expense'
                    }
                  >
                    {transaction.type}
                  </span>
                ),
              },
              {
                key: 'personName',
                header: 'Pessoa',
                render: (transaction) => transaction.personName,
              },
              {
                key: 'createdAt',
                header: 'Data',
                render: (transaction) => formatDate(transaction.createdAt),
              },
            ]}
          />
        )}
      </section>
    </div>
  )
}
