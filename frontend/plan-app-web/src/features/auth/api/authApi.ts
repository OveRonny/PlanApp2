const apiBaseUrl = import.meta.env.VITE_API_URL ?? 'http://localhost:5238'
interface TokenResponse { accessToken: string; refreshToken?: string; expiresIn: number }
interface ApiError { title?: string; detail?: string; errors?: Record<string, string[]> }
async function request<T>(path: string, body: unknown): Promise<T> {
  const response = await fetch(`${apiBaseUrl}${path}`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body) })
  if (!response.ok) { const error = (await response.json().catch(() => ({}))) as ApiError; const validation = error.errors ? Object.values(error.errors).flat().join(' ') : undefined; throw new Error(validation ?? error.detail ?? error.title ?? 'Noe gikk galt. Prøv igjen.') }
  return response.status === 204 ? (undefined as T) : ((await response.json()) as T)
}
export const authApi = { login: (email: string, password: string) => request<TokenResponse>('/api/auth/login', { email, password, twoFactorCode: '' }), register: (email: string, password: string) => request<void>('/api/auth/register', { email, password }) }
export function saveSession(tokens: TokenResponse) { localStorage.setItem('planapp.accessToken', tokens.accessToken); if (tokens.refreshToken) localStorage.setItem('planapp.refreshToken', tokens.refreshToken) }
