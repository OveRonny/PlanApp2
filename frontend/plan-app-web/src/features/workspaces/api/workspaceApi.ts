const apiBaseUrl = import.meta.env.VITE_API_URL ?? 'http://localhost:5238'

export interface Workspace {
  id: string
  name: string
  ownerId: string
  createdAt: string
}

function authHeaders(): HeadersInit {
  const token = localStorage.getItem('planapp.accessToken')
  return token ? { Authorization: `Bearer ${token}` } : {}
}

function handleUnauthorized(response: Response) {
  if (response.status === 401) {
    localStorage.removeItem('planapp.accessToken')
    localStorage.removeItem('planapp.refreshToken')
    window.dispatchEvent(new CustomEvent('planapp:session-expired'))
  }
}

async function readError(response: Response) {
  const body = await response.json().catch(() => ({})) as { title?: string; detail?: string }
  return body.detail ?? body.title ?? 'Noe gikk galt. Prøv igjen.'
}

export async function getWorkspaces(): Promise<Workspace[]> {
  const response = await fetch(`${apiBaseUrl}/api/workspaces`, { headers: authHeaders() })
  handleUnauthorized(response)
  if (!response.ok) throw new Error(await readError(response))
  return response.json() as Promise<Workspace[]>
}

export async function createWorkspace(name: string): Promise<Workspace> {
  const response = await fetch(`${apiBaseUrl}/api/workspaces`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json', ...authHeaders() },
    body: JSON.stringify({ name }),
  })
  handleUnauthorized(response)
  if (!response.ok) throw new Error(await readError(response))
  return response.json() as Promise<Workspace>
}

export async function getWorkspace(id: string): Promise<Workspace> {
  const response = await fetch(`${apiBaseUrl}/api/workspaces/${id}`, { headers: authHeaders() })
  handleUnauthorized(response)
  if (!response.ok) throw new Error(await readError(response))
  return response.json() as Promise<Workspace>
}

export async function updateWorkspace(id: string, name: string): Promise<Workspace> {
  const response = await fetch(`${apiBaseUrl}/api/workspaces/${id}`, {
    method: 'PUT', headers: { 'Content-Type': 'application/json', ...authHeaders() }, body: JSON.stringify({ name }),
  })
  handleUnauthorized(response)
  if (!response.ok) throw new Error(await readError(response))
  return response.json() as Promise<Workspace>
}

export async function deleteWorkspace(id: string): Promise<void> {
  const response = await fetch(`${apiBaseUrl}/api/workspaces/${id}`, { method: 'DELETE', headers: authHeaders() })
  handleUnauthorized(response)
  if (!response.ok) throw new Error(await readError(response))
}
