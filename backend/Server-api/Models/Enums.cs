namespace Server_api.Models;

public enum ProjectRole { Owner, Admin, Member, Viewer }
public enum WorkItemType { Task, Bug, SubTask }
public enum WorkItemStatus { Backlog, Todo, InProgress, InReview, Done, Cancelled }
public enum WorkItemPriority { Low, Medium, High, Critical }
public enum AiSource { Manual, AiGenerated, AiAssisted }
