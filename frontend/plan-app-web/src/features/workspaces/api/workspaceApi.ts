import { apiFetch, readApiError } from '../../../shared/api/apiClient'

export interface Workspace {
  id: string
  name: string
  ownerId: string
  createdAt: string
}

async function json<T>(response: Response): Promise<T> { if (!response.ok) throw new Error(await readApiError(response)); return response.json() as Promise<T> }

export async function getWorkspaces(): Promise<Workspace[]> {
  return json<Workspace[]>(await apiFetch('/api/workspaces'))
}

export async function createWorkspace(name: string): Promise<Workspace> {
  const response = await apiFetch('/api/workspaces', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ name }),
  })
  return json<Workspace>(response)
}

export async function getWorkspace(id: string): Promise<Workspace> {
  return json<Workspace>(await apiFetch(`/api/workspaces/${id}`))
}

export async function updateWorkspace(id: string, name: string): Promise<Workspace> {
  const response = await apiFetch(`/api/workspaces/${id}`, {
    method: 'PUT', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify({ name }),
  })
  return json<Workspace>(response)
}

export async function deleteWorkspace(id: string): Promise<void> {
  const response = await apiFetch(`/api/workspaces/${id}`, { method: 'DELETE' })
  if (!response.ok) throw new Error(await readApiError(response))
}
