import { api } from './api'
import type { CreatePersonRequest, Person } from '../types'

// Agrupa as chamadas de API de pessoas em uma pequena interface tipada.
export const personService = {
  async getPeople(): Promise<Person[]> {
    const { data } = await api.get<Person[]>('/people')
    return data
  },

  async createPerson(payload: CreatePersonRequest): Promise<Person> {
    const { data } = await api.post<Person>('/people', payload)
    return data
  },

  async deletePerson(id: number): Promise<void> {
    await api.delete(`/people/${id}`)
  },
}
