USE [PlanApp2];
GO

DECLARE @UserId uniqueidentifier = '11111111-1111-1111-1111-111111111111';
DECLARE @WorkspaceId uniqueidentifier = '22222222-2222-2222-2222-222222222222';

IF NOT EXISTS (SELECT 1 FROM [Users] WHERE [Id] = @UserId)
BEGIN
    INSERT INTO [Users] ([Id], [Name], [Email], [CreatedAt])
    VALUES (@UserId, N'Test Developer', N'test@planapp.local', SYSUTCDATETIME());
END;

IF NOT EXISTS (SELECT 1 FROM [Workspaces] WHERE [Id] = @WorkspaceId)
BEGIN
    INSERT INTO [Workspaces] ([Id], [Name], [OwnerId], [CreatedAt])
    VALUES (@WorkspaceId, N'Test Workspace', @UserId, SYSUTCDATETIME());
END;

SELECT [Id], [Name], [OwnerId]
FROM [Workspaces]
WHERE [Id] = @WorkspaceId;
