import { api } from './api'
import type { CreateTransactionRequest, FinancialTransaction } from '../types'

// Agrupa as chamadas de API de transacoes e preserva a tipagem das respostas do backend.
export const transactionService = {
  async getTransactions(): Promise<FinancialTransaction[]> {
    const { data } = await api.get<FinancialTransaction[]>('/transactions')
    return data
  },

  async createTransaction(
    payload: CreateTransactionRequest,
  ): Promise<FinancialTransaction> {
    const { data } = await api.post<FinancialTransaction>('/transactions', payload)
    return data
  },
}
