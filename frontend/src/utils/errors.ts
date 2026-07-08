import axios from 'axios'

type ApiErrorBody = {
  message?: string
  error?: string
  title?: string
  errors?: Record<string, string[]>
}

export function getErrorMessage(error: unknown): string {
  // O Axios pode receber formatos diferentes de erro do ASP.NET, validacao ou falhas de rede.
  if (axios.isAxiosError<ApiErrorBody | string>(error)) {
    const data = error.response?.data

    if (typeof data === 'string' && data.trim()) {
      return data
    }

    if (data && typeof data === 'object') {
      if (data.message) return data.message
      if (data.error) return data.error
      if (data.title) return data.title

      // A validacao de modelo do ASP.NET retorna um dicionario de campos para mensagens.
      const validationMessages = data.errors
        ? Object.values(data.errors).flat()
        : []

      if (validationMessages.length > 0) {
        return validationMessages.join(' ')
      }
    }

    if (error.message) {
      return error.message
    }
  }

  if (error instanceof Error) {
    return error.message
  }

  return 'Nao foi possivel concluir a operacao. Tente novamente.'
}
