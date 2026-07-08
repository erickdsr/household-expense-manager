import { api } from './api'
import type { CreateTransactionRequest, FinancialTransaction } from '../types'

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
