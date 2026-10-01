import { readApiError } from '../../../shared/api/apiClient'
export interface TokenResponse { accessToken: string; refreshToken?: string; expiresIn: number }
async function request<T>(path: string, body: unknown): Promise<T> {
  const apiBaseUrl = import.meta.env.VITE_API_URL ?? 'http://localhost:5238'
  const response = await fetch(`${apiBaseUrl}${path}`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body) })
  if (!response.ok) throw new Error(await readApiError(response))
  return response.status === 204 ? (undefined as T) : ((await response.json()) as T)
}
export const authApi = { login: (email: string, password: string) => request<TokenResponse>('/api/auth/login', { email, password, twoFactorCode: '' }), register: (email: string, password: string) => request<void>('/api/auth/register', { email, password }) }
export function saveSession(tokens: TokenResponse) { localStorage.setItem('planapp.accessToken', tokens.accessToken); if (tokens.refreshToken) localStorage.setItem('planapp.refreshToken', tokens.refreshToken) }
export function logout() { localStorage.removeItem('planapp.accessToken'); localStorage.removeItem('planapp.refreshToken'); window.dispatchEvent(new CustomEvent('planapp:logged-out')) }
