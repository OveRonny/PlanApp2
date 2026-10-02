const apiBaseUrl = import.meta.env.VITE_API_URL ?? 'http://localhost:5238'

function clearSession() {
  localStorage.removeItem('planapp.accessToken')
  localStorage.removeItem('planapp.refreshToken')
  window.dispatchEvent(new CustomEvent('planapp:session-expired'))
}

async function refreshSession(): Promise<boolean> {
  const refreshToken = localStorage.getItem('planapp.refreshToken')
  if (!refreshToken) return false
  const response = await fetch(`${apiBaseUrl}/api/auth/refresh`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify({ refreshToken }) })
  if (!response.ok) return false
  const tokens = await response.json() as { accessToken: string; refreshToken?: string }
  localStorage.setItem('planapp.accessToken', tokens.accessToken)
  if (tokens.refreshToken) localStorage.setItem('planapp.refreshToken', tokens.refreshToken)
  return true
}

export async function apiFetch(path: string, init: RequestInit = {}, retry = true, expireSessionOnUnauthorized = true): Promise<Response> {
  const headers = new Headers(init.headers)
  const accessToken = localStorage.getItem('planapp.accessToken')
  if (accessToken) headers.set('Authorization', `Bearer ${accessToken}`)
  const response = await fetch(`${apiBaseUrl}${path}`, { ...init, headers })
  if (response.status === 401 && retry && await refreshSession()) return apiFetch(path, init, false)
  if (response.status === 401 && expireSessionOnUnauthorized) clearSession()
  return response
}

export async function readApiError(response: Response): Promise<string> {
  const body = await response.json().catch(() => ({})) as { title?: string; detail?: string; errors?: Record<string, string[]> }
  const validation = body.errors ? Object.values(body.errors).flat().join(' ') : undefined
  return validation ?? body.detail ?? body.title ?? 'Noe gikk galt. Prøv igjen.'
}
