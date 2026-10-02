import { apiFetch, readApiError } from '../../../shared/api/apiClient'
export type TechnologyCategory = 'Frontend' | 'Backend' | 'Database' | 'Infrastructure' | 'Testing' | 'Integration' | 'Tooling'
export interface Technology { id: string; name: string; category: TechnologyCategory }
export async function getTechnologies() { const response = await apiFetch('/api/technologies'); if (!response.ok) throw new Error(await readApiError(response)); return response.json() as Promise<Technology[]> }
export async function saveProjectTechnologies(projectId: string, technologyIds: string[]) { const response = await apiFetch(`/api/projects/${projectId}/technologies`, { method: 'PUT', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify({ technologyIds }) }); if (!response.ok) throw new Error(await readApiError(response)) }
