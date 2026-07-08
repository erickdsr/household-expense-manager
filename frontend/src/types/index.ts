export interface Person {
  id: number
  name: string
  age: number
}

export interface CreatePersonRequest {
  name: string
  age: number
}

export type TransactionType = 'Expense' | 'Income'

export interface FinancialTransaction {
  id: number
  description: string
  amount: number
  type: TransactionType
  personId: number
  personName: string
  createdAt: string
}

export interface CreateTransactionRequest {
  description: string
  amount: number
  type: TransactionType
  personId: number
}

export interface PersonSummary {
  personId: number
  personName: string
  totalIncome: number
  totalExpenses: number
  balance: number
}

export interface GeneralSummary {
  totalIncome: number
  totalExpenses: number
  netBalance: number
}

export interface SummaryResponse {
  people: PersonSummary[]
  general: GeneralSummary
}
