import { useEffect, useState } from 'react'
import type { FormEvent } from 'react'
import { Button } from '../../components/Button'
import { Input } from '../../components/Input'
import { Table } from '../../components/Table'
import { personService } from '../../services/personService'
import type { Person } from '../../types'
import { getErrorMessage } from '../../utils/errors'

export function People() {
  const [people, setPeople] = useState<Person[]>([])
  const [name, setName] = useState('')
  const [age, setAge] = useState('')
  const [isLoading, setIsLoading] = useState(true)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [error, setError] = useState('')
  const [success, setSuccess] = useState('')

  async function loadPeople() {
    try {
      setIsLoading(true)
      setError('')
      const data = await personService.getPeople()
      setPeople(data)
    } catch (err) {
      setError(getErrorMessage(err))
    } finally {
      setIsLoading(false)
    }
  }

  useEffect(() => {
    async function loadInitialPeople() {
      try {
        setIsLoading(true)
        setError('')
        const data = await personService.getPeople()
        setPeople(data)
      } catch (err) {
        setError(getErrorMessage(err))
      } finally {
        setIsLoading(false)
      }
    }

    void loadInitialPeople()
  }, [])

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    setError('')
    setSuccess('')

    const trimmedName = name.trim()
    const parsedAge = Number(age)

    if (!trimmedName) {
      setError('Nome e obrigatorio.')
      return
    }

    if (age === '' || Number.isNaN(parsedAge)) {
      setError('Idade e obrigatoria.')
      return
    }

    if (parsedAge < 0) {
      setError('Idade nao pode ser negativa.')
      return
    }

    try {
      setIsSubmitting(true)
      await personService.createPerson({ name: trimmedName, age: parsedAge })
      setName('')
      setAge('')
      setSuccess('Pessoa cadastrada com sucesso.')
      await loadPeople()
    } catch (err) {
      setError(getErrorMessage(err))
    } finally {
      setIsSubmitting(false)
    }
  }

  async function handleDelete(person: Person) {
    const confirmed = window.confirm(`Deseja deletar ${person.name}?`)

    if (!confirmed) {
      return
    }

    try {
      setError('')
      setSuccess('')
      await personService.deletePerson(person.id)
      setSuccess('Pessoa deletada com sucesso.')
      await loadPeople()
    } catch (err) {
      setError(getErrorMessage(err))
    }
  }

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <p className="eyebrow">Cadastro</p>
          <h1>Pessoas</h1>
        </div>
      </div>

      {error && <p className="alert alert--error">{error}</p>}
      {success && <p className="alert alert--success">{success}</p>}

      <section className="panel">
        <div className="panel__header">
          <h2>Nova pessoa</h2>
        </div>
        <form className="form-grid" onSubmit={handleSubmit}>
          <Input
            label="Nome"
            name="name"
            type="text"
            value={name}
            onChange={(event) => setName(event.target.value)}
            required
          />
          <Input
            label="Idade"
            name="age"
            type="number"
            min="0"
            value={age}
            onChange={(event) => setAge(event.target.value)}
            required
          />
          <div className="form-actions">
            <Button type="submit" disabled={isSubmitting}>
              {isSubmitting ? 'Salvando...' : 'Cadastrar'}
            </Button>
          </div>
        </form>
      </section>

      <section className="panel">
        <div className="panel__header">
          <h2>Pessoas cadastradas</h2>
        </div>
        {isLoading ? (
          <p className="status-message">Carregando pessoas...</p>
        ) : (
          <Table<Person>
            data={people}
            emptyMessage="Nenhuma pessoa cadastrada."
            getRowKey={(person) => person.id}
            columns={[
              {
                key: 'id',
                header: 'ID',
                render: (person) => person.id,
              },
              {
                key: 'name',
                header: 'Nome',
                render: (person) => person.name,
              },
              {
                key: 'age',
                header: 'Idade',
                render: (person) => person.age,
              },
              {
                key: 'actions',
                header: 'Acoes',
                align: 'right',
                render: (person) => (
                  <Button
                    type="button"
                    variant="danger"
                    onClick={() => void handleDelete(person)}
                  >
                    Deletar
                  </Button>
                ),
              },
            ]}
          />
        )}
      </section>
    </div>
  )
}
