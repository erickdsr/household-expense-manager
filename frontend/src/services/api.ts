import axios from 'axios'

// Cliente HTTP compartilhado para manter a URL base da API em um unico lugar.
export const api = axios.create({
  baseURL: 'https://localhost:5001/api',
  headers: {
    'Content-Type': 'application/json',
  },
})
