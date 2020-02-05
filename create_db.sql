create table Lists(
    Id    int          identity,
    Title nvarchar(30) not null,

    constraint [PK_Lists_Id] primary key (Id),
)

create unique index [UQ_Lists_Title] on Lists (Title)

create table Tasks(
    Id          int          identity,
    Title       nvarchar(30) not null,
    Description nvarchar(100) not null,
    Importance  tinyint      not null,
    Deadline    datetime         null,
    IsCompleted bit          not null,
    IsDeleted   bit          not null,
    Created     datetime     not null default (getdate()),
    ListId      int          not null,

    constraint [PK_Tasks_Id] primary key (Id),
    constraint [FK_Tasks_ListId] foreign key (ListId) references Lists(id) on delete cascade
)
go
