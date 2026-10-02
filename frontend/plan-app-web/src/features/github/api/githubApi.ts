import { apiFetch, readApiError } from '../../../shared/api/apiClient'

const clientId = import.meta.env.VITE_GITHUB_CLIENT_ID ?? ''
const redirectUri = `${window.location.origin}/github/callback`

export function startGitHubOAuth(workspaceId: string) {
  if (!clientId) throw new Error('VITE_GITHUB_CLIENT_ID mangler i frontend-konfigurasjonen.')
  localStorage.setItem('planapp.githubWorkspaceId', workspaceId)
  const params = new URLSearchParams({ client_id: clientId, redirect_uri: redirectUri, scope: 'read:user repo', state: crypto.randomUUID() })
  window.location.assign(`https://github.com/login/oauth/authorize?${params}`)
}

export async function connectGitHub(code: string) {
  const response = await apiFetch('/api/github/connection', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify({ code }) })
  if (!response.ok) throw new Error(await readApiError(response))
  return response.json() as Promise<{ id: string; gitHubUserId: string; gitHubLogin: string }>
}

export interface GitHubRepository { id: number; fullName: string; defaultBranch?: string }
export interface GitHubCollaborator { id: number; login: string; name?: string; avatarUrl?: string }
export async function getRepositories() { const response = await apiFetch('/api/github/repositories', {}, false, false); if (!response.ok) throw new Error(await readApiError(response)); return response.json() as Promise<GitHubRepository[]> }
export async function getCollaborators(repository: GitHubRepository) { const response = await apiFetch(`/api/github/repositories/${repository.id}/collaborators?fullName=${encodeURIComponent(repository.fullName)}`, {}, false, false); if (!response.ok) throw new Error(await readApiError(response)); return response.json() as Promise<GitHubCollaborator[]> }

export function clearGitHubCallback() {
  const url = new URL(window.location.href)
  url.search = ''
  url.pathname = '/'
  window.history.replaceState({}, document.title, url.toString())
}
