import { api } from './api'
import type { SummaryResponse } from '../types'

export const summaryService = {
  async getSummary(): Promise<SummaryResponse> {
    const { data } = await api.get<SummaryResponse>('/summary')
    return data
  },
}
