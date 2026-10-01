import { apiFetch, readApiError } from '../../../shared/api/apiClient'
export interface Project { id: string; workspaceId: string; name: string; description?: string | null; createdAt: string }
async function json<T>(response: Response): Promise<T> { if (!response.ok) throw new Error(await readApiError(response)); return response.json() as Promise<T> }
export async function getProjects(workspaceId: string) { return json<Project[]>(await apiFetch(`/api/projects?workspaceId=${workspaceId}`)) }
export async function createProject(workspaceId: string, name: string, description?: string) { return json<Project>(await apiFetch('/api/projects', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify({ workspaceId, name, description: description || null }) })) }
export async function updateProject(id: string, name: string, description?: string) { return json<Project>(await apiFetch(`/api/projects/${id}`, { method: 'PUT', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify({ name, description: description || null }) })) }
export async function deleteProject(id: string) { const response = await apiFetch(`/api/projects/${id}`, { method: 'DELETE' }); if (!response.ok) throw new Error(await readApiError(response)) }
