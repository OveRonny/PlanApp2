import { apiFetch, readApiError } from '../../../shared/api/apiClient'
export interface ProjectTask { id: string; title: string; description?: string | null; status: string; priority: string; type: string; featureId?: string | null; featureTitle?: string | null; aiSource: string }
export async function getProjectTasks(projectId: string) { const response = await apiFetch(`/api/projects/${projectId}/tasks`); if (!response.ok) throw new Error(await readApiError(response)); return response.json() as Promise<ProjectTask[]> }
