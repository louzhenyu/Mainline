USE FxDB
GO
if exists (select 1
            from  sysobjects
           where  id = object_id('JobHttpScheduler')
            and   type = 'U')
   drop table JobHttpScheduler
go

/*==============================================================*/
/* Table: JobHttpScheduler                                      */
/*==============================================================*/
create table JobHttpScheduler (
   JobHttpSchedulerID   int                  identity,
   JobName              varchar(100)         not null,
   GroupName            varchar(100)         not null default 'DefaultGroup',
   RequestURL           varchar(100)         not null,
   RequestType          int                  not null default 0,
   JobDescription       nvarchar(200)        not null default '',
   StartTime            datetime             not null default getdate(),
   TriggerType          int                  not null default 0,
   RepeatCount          int                  not null default -1,
   RepeatInterval       int                  not null default 0,
   CronExpression       varchar(200)         not null,
   JobStatus            int                  not null default 0,
   AddTime              datetime             not null default getdate(),
   constraint PK_JOBHTTPSCHEDULER primary key (JobHttpSchedulerID)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '计划任务表',
   'user', @CurrentUser, 'table', 'JobHttpScheduler'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '主键',
   'user', @CurrentUser, 'table', 'JobHttpScheduler', 'column', 'JobHttpSchedulerID'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Job名称，必须唯一',
   'user', @CurrentUser, 'table', 'JobHttpScheduler', 'column', 'JobName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '组名',
   'user', @CurrentUser, 'table', 'JobHttpScheduler', 'column', 'GroupName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '请求地址，完成的URL地址，例如：http://www.jinri.cn/Example.aspx',
   'user', @CurrentUser, 'table', 'JobHttpScheduler', 'column', 'RequestURL'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '任务开始时间',
   'user', @CurrentUser, 'table', 'JobHttpScheduler', 'column', 'StartTime'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Trigger类型，0：SimpleTrigger，1：CronTrigger',
   'user', @CurrentUser, 'table', 'JobHttpScheduler', 'column', 'TriggerType'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'SimpleTrigger重复执行次数，-1表示无限次，其他正整数表示具体重复的次数',
   'user', @CurrentUser, 'table', 'JobHttpScheduler', 'column', 'RepeatCount'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'SimpleTrigger重复执行间隔时间，单位：秒',
   'user', @CurrentUser, 'table', 'JobHttpScheduler', 'column', 'RepeatInterval'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Cron表达式',
   'user', @CurrentUser, 'table', 'JobHttpScheduler', 'column', 'CronExpression'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '0:开启，1：暂停',
   'user', @CurrentUser, 'table', 'JobHttpScheduler', 'column', 'JobStatus'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '添加时间',
   'user', @CurrentUser, 'table', 'JobHttpScheduler', 'column', 'AddTime'
go
